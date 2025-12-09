using MapOfDiet.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MapOfDiet.Services
{
    internal static class DBWork
    {
        private static string connString = ConfigurationManager.ConnectionStrings["PostgresConn"].ConnectionString;

        //// AuthWindow ----------------------------------------------------------------------------------
        
        // Получить по логину всю инфу по информации для аунтефикации
        public static UserAuth? getUserModel(string login)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT user_id, hash, salt, status FROM users_auth WHERE login = @username", conn))
                {
                    cmd.Parameters.AddWithValue("username", login);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new UserAuth
                            {
                                Login = login,
                                UserId = reader.GetInt32(0),
                                Hash = reader.GetFieldValue<byte[]>(1),
                                Salt = reader.GetFieldValue<byte[]>(2),
                                Status = reader.GetBoolean(3)
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Отправить в бд инфу о аккаунте
        public static bool pushNewUserAuth(UserAuth userAuth)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        using (var check = new NpgsqlCommand("SELECT 1 FROM users_auth WHERE login = @login", conn, tran))
                        {
                            check.Parameters.AddWithValue("login", userAuth.Login);
                            var exists = check.ExecuteScalar();
                            if (exists != null)
                            {
                                tran.Rollback();
                                return false;
                            }
                        }

                        int userID;
                        using (var cmd = new NpgsqlCommand("INSERT INTO users DEFAULT VALUES RETURNING user_id", conn, tran))
                        {
                            userID = (int)cmd.ExecuteScalar();
                        }

                        using (var cmd = new NpgsqlCommand("INSERT INTO users_auth (user_id, login, hash, salt) VALUES (@user_id, @login, @hash, @salt)", conn, tran))
                        {
                            cmd.Parameters.AddWithValue("user_id", userID);
                            cmd.Parameters.AddWithValue("login", userAuth.Login);
                            cmd.Parameters.AddWithValue("hash", userAuth.Hash);
                            cmd.Parameters.AddWithValue("salt", userAuth.Salt);
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                        return true;
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            }
        }

        // Проверить админ ли пользователь
        public static bool checkUserStatus(int userId)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT status FROM users_auth WHERE user_id = @user_id", conn))
                {
                    cmd.Parameters.AddWithValue("user_id", userId);
                    var result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        return false;

                    return Convert.ToBoolean(result);
                }
            }
        }

        // Получить id по логину
        public static int getUserId(string login)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT user_id FROM users_auth WHERE login = @login", conn))
                {
                    cmd.Parameters.AddWithValue("login", login);
                    var result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        return -2;

                    return Convert.ToInt32(result);
                }
            }
        }

        //// AdminWindow ----------------------------------------------------------------------------------

        // Добавляет новую еду в бд
        public static void AddRecipe(Food recipe)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO foods (name, standard_mass, calories, proteins, fats, carbohydrates, description) " +
                "VALUES (@name, @mass, @calories, @proteins, @fats, @carbohydrates, @desc) RETURNING food_id", conn);

            cmd.Parameters.AddWithValue("name", recipe.Name);
            cmd.Parameters.AddWithValue("mass", recipe.Mass > 0 ? recipe.Mass : 100);
            cmd.Parameters.AddWithValue("calories", recipe.Calories);
            cmd.Parameters.AddWithValue("proteins", recipe.Proteins);
            cmd.Parameters.AddWithValue("fats", recipe.Fats);
            cmd.Parameters.AddWithValue("carbohydrates", recipe.Carbohydrates);
            cmd.Parameters.AddWithValue("desc", recipe.Description ?? "");

            var scalar = cmd.ExecuteScalar();
            if (scalar is null || scalar == DBNull.Value)
                throw new InvalidOperationException("Не удалось получить идентификатор блюда.");

            int foodId = Convert.ToInt32(scalar);

            foreach (var cat in recipe.Categories)
            {
                using var cmdCat = new NpgsqlCommand(
                    "INSERT INTO foods_categories (food_id, category_id) VALUES (@food_id, @cat_id)", conn);
                cmdCat.Parameters.AddWithValue("food_id", foodId);
                cmdCat.Parameters.AddWithValue("cat_id", cat.CategoryId);
                cmdCat.ExecuteNonQuery();
            }

            foreach (var ingr in recipe.Ingredients)
            {
                using var cmdIngr = new NpgsqlCommand(
                    "INSERT INTO foods_ingredients (food_id, ingr_id, measure_value) VALUES (@food_id, @ingr_id, @mass)", conn);
                cmdIngr.Parameters.AddWithValue("food_id", foodId);
                cmdIngr.Parameters.AddWithValue("ingr_id", ingr.IngrId);
                cmdIngr.Parameters.AddWithValue("mass", ingr.Mass);
                cmdIngr.ExecuteNonQuery();
            }
        }

        // Найти список похожих на это название ингридиентов
        public static List<Ingredient> SearchIngredientsByName(string name)
        {
            var list = new List<Ingredient>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT ingr_id, name, description, measure_name " +
                "FROM ingredients " +
                "WHERE name ILIKE @name " +
                "ORDER BY name LIMIT 10", conn);

            cmd.Parameters.AddWithValue("name", "%" + (name ?? string.Empty) + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Ingredient
                {
                    IngrId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    MeasureName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    Mass = 0
                });
            }

            return list;
        }

        // Найти список похожих на это название категорий
        public static List<Category> SearchCategoriesByName(string name)
        {
            var list = new List<Category>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT category_id, name, description " +
                "FROM categories " +
                "WHERE name ILIKE @name " +
                "ORDER BY name LIMIT 10", conn);

            cmd.Parameters.AddWithValue("name", "%" + (name ?? string.Empty) + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Category
                {
                    CategoryId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                });
            }

            return list;
        }

        // Отправить в бд новую категорию
        public static bool pushNewCategory(Category category)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO categories (name, description) VALUES (@name, @description) ON CONFLICT (name) DO NOTHING RETURNING name", conn);
            cmd.Parameters.AddWithValue("name", category.Name);
            cmd.Parameters.AddWithValue("description", category.Description);
            var result = cmd.ExecuteScalar();
            return result != null;
        }

        // Отправить в бд новый ингридиент
        public static bool pushNewIngredient(Ingredient ingredient)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO ingredients (name, description, measure_name) VALUES (@name, @description, @measName) ON CONFLICT (name) DO NOTHING RETURNING name", conn);
            cmd.Parameters.AddWithValue("name", ingredient.Name);
            cmd.Parameters.AddWithValue("description", ingredient.Description);
            cmd.Parameters.AddWithValue("measName", ingredient.MeasureName);
            var result = cmd.ExecuteScalar();
            return result != null;
        }

        // Отправить в бд новую физическую активность
        public static bool pushNewActivity(MyActivity activity)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO activities (name, description, measure_name, measure_to_calories) VALUES (@name, @description, @measName, @measCal) ON CONFLICT (name) DO NOTHING RETURNING name", conn);
            cmd.Parameters.AddWithValue("name", activity.Name);
            cmd.Parameters.AddWithValue("description", activity.Description);
            cmd.Parameters.AddWithValue("measName", activity.MeasureName);
            cmd.Parameters.AddWithValue("measCal", activity.MeasureToCalories);
            var result = cmd.ExecuteScalar();
            return result != null;
        }

        //// Profile ----------------------------------------------------------------------------------
        
        // При изменении веса текущего или ожидаемого пушить его в историю весов с текущей датой (то есть пушится не когда удаляются, а когда создаются данные)
        public static bool pushNewWeightRecord(WeightRecord weightRecord)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO weight_history (user_id, time, weight, target_weight) VALUES (@user_id, @time, @weight, @target_weight)", conn);
            cmd.Parameters.AddWithValue("user_id", UserSession.UserId);
            cmd.Parameters.AddWithValue("time", weightRecord.Date);
            cmd.Parameters.AddWithValue("weight", weightRecord.Weight);
            cmd.Parameters.AddWithValue("target_weight", weightRecord.TargetWeight);
            return true;
        }

        public static async Task<bool> PushUserProfileAsync(UserProfile userProfile)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            await using var tran = await conn.BeginTransactionAsync();
            try
            {
                // --- Обновляем таблицу users ---
                await using (var cmd = new NpgsqlCommand(@"UPDATE users SET name = @name,height = @height, age = @age, gender = @genderWHERE user_id = @user_id", conn, tran))                {
                    cmd.Parameters.AddWithValue("name", userProfile.Name);
                    cmd.Parameters.AddWithValue("height", userProfile.Height);
                    cmd.Parameters.AddWithValue("age", userProfile.Age);
                    cmd.Parameters.AddWithValue("gender", userProfile.Gender);
                    cmd.Parameters.AddWithValue("user_id", userProfile.UserId);

                    await cmd.ExecuteNonQueryAsync();
                }

                // --- Добавляем новую запись в weight_history ---
                await using (var cmd = new NpgsqlCommand("INSERT INTO weight_history (user_id, time, weight, target_weight) VALUES (@user_id, @time, @weight, @target_weight)", conn, tran))
                {
                    cmd.Parameters.AddWithValue("user_id", userProfile.UserId);
                    cmd.Parameters.AddWithValue("time", DateTime.Now);
                    cmd.Parameters.AddWithValue("weight", userProfile.NowWeight);
                    cmd.Parameters.AddWithValue("target_weight", userProfile.TargetWeight);

                    await cmd.ExecuteNonQueryAsync();
                }

                // --- Обновляем предпочтения категорий ---
                // Сначала удалим старые записи
                await using (var cmdDel = new NpgsqlCommand(
                    "DELETE FROM users_preferences WHERE user_id = @user_id", conn, tran))
                {
                    cmdDel.Parameters.AddWithValue("user_id", userProfile.UserId);
                    await cmdDel.ExecuteNonQueryAsync();
                }

                // Добавляем новые лайки
                foreach (var cat in userProfile.LikeCategories)
                {
                    await using var cmdIns = new NpgsqlCommand(@"INSERT INTO users_preferences (user_id, category_id, prefer) VALUES (@user_id, @cat_id, TRUE)", conn, tran);

                    cmdIns.Parameters.AddWithValue("user_id", userProfile.UserId);
                    cmdIns.Parameters.AddWithValue("cat_id", cat.CategoryId);
                    await cmdIns.ExecuteNonQueryAsync();
                }

                // Добавляем новые дизлайки
                foreach (var cat in userProfile.DislikeCategories)
                {
                    await using var cmdIns = new NpgsqlCommand(@"INSERT INTO users_preferences (user_id, category_id, prefer) VALUES (@user_id, @cat_id, FALSE)", conn, tran);

                    cmdIns.Parameters.AddWithValue("user_id", userProfile.UserId);
                    cmdIns.Parameters.AddWithValue("cat_id", cat.CategoryId);
                    await cmdIns.ExecuteNonQueryAsync();
                }

                // --- Коммит транзакции ---
                await tran.CommitAsync();
                return true;
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        //// Recipe ----------------------------------------------------------------------------------

        // Получить все категории привязанные к еде
        public static List<Category> GetAllCategoriesByFoodId(int foodId)
        {
            var result = new List<Category>();
            var categoriesIds = new List<int>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT category_id FROM foods_categories WHERE food_id = @FoodId", conn))
            {
                cmd.Parameters.AddWithValue("FoodId", foodId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categoriesIds.Add(reader.GetInt32(0));
                }
            }

            if (categoriesIds.Count == 0) { return result; }

            using (var cmd = new NpgsqlCommand("SELECT category_id, name, description FROM categories WHERE category_id = ANY(@CatId)", conn))
            {
                cmd.Parameters.AddWithValue("CatId", categoriesIds);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new Category
                    {
                        CategoryId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2)
                    });
                }
            }


            return result;
        }

        // Получить все ингридиенты привязанные к еде
        public static List<Ingredient> GetAllIngredientByFoodId(int foodId)
        {
            var result = new List<Ingredient>();
            var ingredientsMass = new Dictionary<int, double>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT ingr_id, measure_value FROM foods_ingredients WHERE food_id = @FoodId", conn))
            {
                cmd.Parameters.AddWithValue("FoodId", foodId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ingredientsMass[reader.GetInt32(0)] = reader.GetDouble(1);
                }
            }

            if (ingredientsMass.Count == 0) { return result; }

            using (var cmd = new NpgsqlCommand("SELECT ingr_id, name, description, measure_name FROM ingredients WHERE ingr_id = ANY(@IngrId)", conn))
            {
                cmd.Parameters.AddWithValue("IngrId", ingredientsMass.Keys.ToList());
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ingrId = reader.GetInt32(0);
                    result.Add(new Ingredient
                    {
                        IngrId = ingrId,
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        MeasureName = reader.GetString(3),
                        Mass = ingredientsMass[ingrId]
                    });
                }
            }
            return result;
        }

        // Вызывает другой метод GetFoodById (получает id по name)
        public static Food? GetFoodByName(string name)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT food_id FROM foods WHERE name = @Name", conn);
            cmd.Parameters.AddWithValue("Name", name);
            object? OFoodId = cmd.ExecuteScalar();
            if (OFoodId != null)
            {
                return GetFoodByID((int)OFoodId);
            }
            return null;
        }

        // Получает бизнес логику Food по Id
        public static Food? GetFoodByID(int foodId)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT name, calories, proteins, fats, carbohydrates, description FROM foods WHERE food_id = @FoodId", conn);
            cmd.Parameters.AddWithValue("FoodId", foodId);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var food = new Food
                {
                    Name = reader.GetString(0),
                    Calories = reader.GetDouble(1),
                    Proteins = reader.GetDouble(2),
                    Fats = reader.GetDouble(3),
                    Carbohydrates = reader.GetDouble(4),
                    Description = reader.GetString(5),
                };
                reader.Close();

                food.Categories = GetAllCategoriesByFoodId(foodId);
                food.Ingredients = GetAllIngredientByFoodId(foodId);
                return food;
            }
            return null;
        }

        public static List<Food> SearchFoodsByName(string name)
        {
            var list = new List<Food>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT food_id, name, description, calories, proteins, fats, carbohydrates " +
                "FROM foods " +
                "WHERE name ILIKE @name " +
                "ORDER BY name LIMIT 10", conn);

            cmd.Parameters.AddWithValue("name", "%" + (name ?? string.Empty) + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int FoodId = reader.GetInt32(0);
                list.Add(new Food
                {
                    FoodId = FoodId,
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    Calories = reader.IsDBNull(3) ? 0 : reader.GetDouble(3),
                    Proteins = reader.IsDBNull(4) ? 0 : reader.GetDouble(4),
                    Fats = reader.IsDBNull(5) ? 0 : reader.GetDouble(5),
                    Carbohydrates = reader.IsDBNull(6) ? 0 : reader.GetDouble(6),
                    Mass = 100, 
                    Categories = GetAllCategoriesByFoodId(FoodId), 
                    Ingredients = GetAllIngredientByFoodId(FoodId) 
                });
            }

            return list;
        }


        //// AddFoodRecord ----------------------------------------------------------------------------------

        public static bool PushFoodRecord(FoodRecord foodRecord)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO foods_history (user_id, food_id, mass, eaten_at) VALUES (@user_id, @food_id, @mass, @eaten_at)", conn);
            cmd.Parameters.AddWithValue("user_id", UserSession.UserId);
            cmd.Parameters.AddWithValue("food_id", foodRecord.Food.FoodId);
            cmd.Parameters.AddWithValue("mass", foodRecord.Mass);
            cmd.Parameters.AddWithValue("eaten_at", foodRecord.DateTime);
            return true;
        }

        //// AddActivityRecord ----------------------------------------------------------------------------------

        public static bool PushActivityRecord(MyActivityRecord activityRecord)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO activities_history (user_id, activity_id, measure_value, performed_at) VALUES (@user_id, @activity_id, @value, @time)", conn);
            cmd.Parameters.AddWithValue("user_id", UserSession.UserId);
            cmd.Parameters.AddWithValue("activity_id", activityRecord.Activity.ActivityId);
            cmd.Parameters.AddWithValue("value", activityRecord.Amount);
            cmd.Parameters.AddWithValue("time", activityRecord.DateTime);
            return true;
        }

        //// Statistics ----------------------------------------------------------------------------------

        // Получает данные о изменениях веса у текущего пользователя
        public static List<WeightRecord> GetWeightHistory(int userId)
        {
            var result = new List<WeightRecord>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT time, weight, target_weight FROM weight_history WHERE user_id = @user_id", conn);
            cmd.Parameters.AddWithValue("user_id", userId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new WeightRecord
                {
                    Date = DateOnly.FromDateTime(reader.GetDateTime(0)),
                    Weight = reader.GetDouble(1),
                    TargetWeight = reader.GetDouble(2)
                });
            }
            return result;
        }

        // Получает данные о истории еды у текущего пользователя (не дописано)
        public static List<FoodRecord> GetFoodHistory(int userId)
        {
            var result = new List<FoodRecord>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT time, weight, target_weight FROM weight_history WHERE user_id = @user_id", conn);
            cmd.Parameters.AddWithValue("user_id", userId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new FoodRecord
                {
                    
                });
            }
            return result;
        }

        //// GetPlan (план получается не из бд, он там не хранится) ----------------------------------------------------------------------------------

        public static async Task<UserProfile?> GetUserProfileAsync(int userId)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            try
            {
                UserProfile userProfile = new();

                await using (var cmd = new NpgsqlCommand("SELECT name, height, date_birth, gender FROM users WHERE user_id = @user_id", conn))
                {
                    cmd.Parameters.AddWithValue("user_id", userId);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        userProfile.Name = reader.GetString(0);
                        userProfile.Height = reader.GetInt32(1);

                        var birthDate = reader.GetDateTime(2);
                        userProfile.Age = DateTime.Now.Year - birthDate.Year;
                        if (DateTime.Now.Date < birthDate.AddYears(userProfile.Age)) userProfile.Age--; // корректировка по месяцу/дню

                        userProfile.Gender = reader.GetChar(3);
                    }
                }

                // Получаем вес
                var latest = await GetLatestWeightAsync(userId);
                if (latest != null)
                {
                    userProfile.NowWeight = latest.Value.Current;
                    userProfile.TargetWeight = latest.Value.Target;
                }

                // Получаем категории
                var categories = await GetCategoriesByUserIdAsync(userId);
                userProfile.LikeCategories = categories.LikeCategories;
                userProfile.DislikeCategories = categories.DislikeCategories;

                return userProfile;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<(double Current, double Target)?> GetLatestWeightAsync(int userId)
        {
            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand("SELECT weight, target_weight FROM weight_history WHERE user_id = @UserId ORDER BY time DESC LIMIT 1", conn);

            cmd.Parameters.AddWithValue("UserId", userId);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return (reader.GetDouble(0), reader.GetDouble(1));
            }

            return null;
        }

        public static async Task<(List<Category> LikeCategories, List<Category> DislikeCategories)> GetCategoriesByUserIdAsync(int userId)
        {
            var likeCategories = new List<Category>();
            var dislikeCategories = new List<Category>();

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Получаем все категории пользователя
            await using var cmd = new NpgsqlCommand(@"SELECT c.category_id, c.name, c.description, up.prefer FROM users_preferences up JOIN categories c ON up.category_id = c.category_id WHERE up.user_id = @userId", conn);

            cmd.Parameters.AddWithValue("userId", userId);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var category = new Category
                {
                    CategoryId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2)
                };

                bool prefer = reader.GetBoolean(3);
                if (prefer)
                    likeCategories.Add(category);
                else
                    dislikeCategories.Add(category);
            }

            return (likeCategories, dislikeCategories);
        }

    }
}
