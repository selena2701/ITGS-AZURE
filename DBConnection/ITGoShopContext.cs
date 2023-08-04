using ITGoShop_F_Ver2.Controllers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Models
{
    public class ITGoShopContext
    {
        public string ConnectionString { get; set; }

        public ITGoShopContext(string connectionString) 
        {
            this.ConnectionString = connectionString;
        }

        public ITGoShopContext()
        {
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public User getUserInfo(string email, string password, int isAdmin)
        {
            User userInfo = new User();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "select * from User where Admin = @admin AND Email = @email AND Password = @password";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("email", email);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("admin", isAdmin);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userInfo.UserId = Convert.ToInt32(reader["UserId"]);
                        userInfo.FirstName = reader["FirstName"].ToString();
                        userInfo.LastName = reader["LastName"].ToString();
                        userInfo.Mobile = reader["Mobile"].ToString();
                        userInfo.UserImage = reader["UserImage"].ToString();
                        userInfo.Email = reader["Email"].ToString();
                        userInfo.Admin = Convert.ToInt32(reader["Admin"]);
                    }
                    else
                        return null;
                }
            }
            return userInfo;
        }

        public void updateLastLogin(int userId)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "UPDATE USER SET LastLogin = SYSDATE() where UserId = @userId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.ExecuteNonQuery();
            }
        }

        public int countCustomer()
        {
            int numberCustomer = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_CUSTOMER FROM USER WHERE ADMIN = 0";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberCustomer = Convert.ToInt32(reader["NUMBER_CUSTOMER"]);
                    }
                }
            }
            return numberCustomer;
        }

        public int countProduct()
        {
            int numberProduct = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_PRODUCT FROM PRODUCT";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberProduct = Convert.ToInt32(reader["NUMBER_PRODUCT"]);
                    }
                }
            }
            return numberProduct;
        }

        public int countOrder()
        {
            int numberOrder = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_ORDER FROM `ORDER` WHERE ORDERSTATUS <> 'Đã hủy'";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberOrder = Convert.ToInt32(reader["NUMBER_ORDER"]);
                    }
                }
            }
            return numberOrder;
        }

        public int countOrder(DateTime startDate, DateTime endDate)
        {
            int numberOrder = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_ORDER FROM `ORDER` WHERE ORDERSTATUS <> 'Đã hủy' AND (DATE(ORDERDATE) BETWEEN @startdate AND @enddate)";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberOrder = Convert.ToInt32(reader["NUMBER_ORDER"]);
                    }
                }
            }
            return numberOrder;
        }

        public int countLoginThisYear()
        {
            int numberLoginThisYear = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS Number_LoginThisYear FROM `loginhistory` WHERE YEAR(LoginDate) = YEAR(SYSDATE())";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberLoginThisYear = Convert.ToInt32(reader["Number_LoginThisYear"]);
                    }
                }
            }
            return numberLoginThisYear;
        }

        public int countLogin(DateTime startDate, DateTime endDate)
        {
            int numberLogin = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS Number_Login FROM `loginhistory` WHERE LOGINDATE BETWEEN @startdate AND @enddate";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numberLogin = Convert.ToInt32(reader["Number_Login"]);
                    }
                }
            }
            return numberLogin;
        }

        public List<object> countLoginByDate(DateTime startDate, DateTime endDate)
        {
            List<object> loginHistory = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS NUMBER_LOGIN, LoginDate FROM loginhistory WHERE LOGINDATE BETWEEN @startdate AND @enddate GROUP BY LoginDate";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            period = ((DateTime)reader["LoginDate"]).ToString("dd-MM-yyyy"),
                            number_access = Convert.ToInt32(reader["NUMBER_LOGIN"]),
                        };
                        loginHistory.Add(obj);
                    }
                }
            }
            return loginHistory;
        }

        public long getRevenue(DateTime startDate, DateTime endDate)
        {
            int revenue = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT SUM(PROFIT) AS TOTAL_REVENUE FROM STATISTIC WHERE STATISTICDATE BETWEEN @startdate AND @enddate";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        revenue = Convert.ToInt32(reader["TOTAL_REVENUE"]);
                    }
                }
            }
            return revenue;
        }
        public List<object> getTopProduct(DateTime startDate, DateTime endDate)
        {
            List<object> products = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT SUM(OrderQuantity) AS NumberSolded , ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "FROM (`product` P JOIN `orderdetail` OD ON P.ProductId = OD.ProductId) " +
                    "JOIN `order` O ON O.OrderId = OD.OrderId " +
                    "WHERE OrderStatus <> 'Đã hủy' " +
                    "AND ORDERDATE BETWEEN @startdate AND @enddate " +
                    "GROUP BY ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "ORDER BY SUM(OrderQuantity) DESC " +
                    "LIMIT 5;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Cost = Convert.ToInt32(reader["Cost"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            NumberSolded = Convert.ToInt32(reader["NumberSolded"])
                        };
                        products.Add(obj);
                    }
                }
            }
            return products;
        }

        public List<object> getTopProductNotLimit(DateTime startDate, DateTime endDate)
        {
            List<object> products = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT SUM(OrderQuantity) AS NumberSolded , ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "FROM (`product` P JOIN `orderdetail` OD ON P.ProductId = OD.ProductId) " +
                    "JOIN `order` O ON O.OrderId = OD.OrderId " +
                    "WHERE OrderStatus <> 'Đã hủy' " +
                    "AND ORDERDATE BETWEEN @startdate AND @enddate " +
                    "GROUP BY ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "ORDER BY SUM(OrderQuantity) DESC ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Cost = Convert.ToInt32(reader["Cost"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            NumberSolded = Convert.ToInt32(reader["NumberSolded"])
                        };
                        products.Add(obj);
                    }
                }
            }
            return products;
        }

        public List<Blog> getTopBlogView()
        {
            List<Blog> blogs = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM BLOG ORDER BY VIEW DESC LIMIT 5";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blogs.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            DateCreate = (DateTime)reader["DateCreate"],
                            Image = reader["Image"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                        });
                    }
                }
            }
            return blogs;
        }

        public List<Product> getTopProductView()
        {
            List<Product> products = new List<Product>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM PRODUCT ORDER BY VIEW DESC LIMIT 5";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("1");
                        products.Add(new Product()
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            View = Convert.ToInt32(reader["View"]),
                        });
                    }
                }
            }
            return products;
        }

        public List<Product> getInventoryList()
        {
            List<Product> products = new List<Product>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM PRODUCT ORDER BY SOLD DESC, StartsAt ASC LIMIT 5;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product()
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            View = Convert.ToInt32(reader["View"]),
                        });
                    }
                }
            }
            return products;
        }
        public List<object> countOrderByDate(DateTime startDate, DateTime endDate)
        {
            List<object> ordersInfo = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT COUNT(*) AS Number_Order, OrderStatus " +
                    "FROM `ORDER` " +
                    "WHERE OrderDate BETWEEN @startdate AND @enddate " +
                    "GROUP BY OrderStatus";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("1");
                        var obj = new
                        {
                            label = reader["OrderStatus"].ToString(),
                            value = Convert.ToInt32(reader["Number_Order"]),
                        };
                        ordersInfo.Add(obj);
                    }
                }
            }
            return ordersInfo;
        }

        public List<object> getRevenueByDate(DateTime startDate, DateTime endDate)
        {
            List<object> revenueInfo = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * " +
                    "FROM STATISTIC " +
                    "WHERE STATISTICDATE BETWEEN @startdate AND @enddate " +
                    "ORDER BY STATISTICDATE ASC";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", startDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("enddate", endDate.ToString("yyyy-MM-dd"));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            period = ((DateTime)reader["StatisticDate"]).ToString("dd-MM-yyyy"),
                            sales = Convert.ToInt32(reader["Sales"]),
                            profit = Convert.ToInt32(reader["Profit"]),
                        };
                        revenueInfo.Add(obj);
                    }
                }
            }
            return revenueInfo;
        }

        public List<Category> getAllCategory()
        {
            List<Category> list = new List<Category>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from Category where Status = 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Category()
                        {
                            CategoryId = reader["CategoryId"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                            
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<SubBrand> getAllSubBrand()
        {
            List<SubBrand> list = new List<SubBrand>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from SubBrand where Status = 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SubBrand()
                        {
                            SubBrandId = reader["SubBrandId"].ToString(),
                            SubBrandName = reader["SubBrandName"].ToString(),
                            BrandId = Convert.ToInt32(reader["BrandId"])
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<Brand> getAllBrand()
        {
            List<Brand> list = new List<Brand>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from Brand where Status = 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Brand()
                        {
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            BrandName = reader["BrandName"].ToString(),
                            Description = reader["Description"].ToString(),
                            CategoryId = reader["CategoryId"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                            BrandLogo = reader["BrandLogo"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getAllBrandForBrandManagement()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select BrandId, CategoryName, BrandName, B.Status, BrandLogo, B.Description, B.CategoryId " +
                    " from Brand B JOIN Category C ON B.CategoryId = C.CategoryId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new 
                        {
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            BrandName = reader["BrandName"].ToString(),
                            Description = reader["Description"].ToString(),
                            CategoryId = reader["CategoryId"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                            BrandLogo = reader["BrandLogo"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<Category> getAllCateForCateManagement()
        {
            List<Category> list = new List<Category>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from Category ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Category()
                        {
                            CategoryId = reader["CategoryId"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),

                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<Blog> getAllBlogForBlogManagement()
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            Image = reader["Image"].ToString(),
                            Status = Convert.ToInt32(reader["Status"])

                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<object> getAllSubBrandForBrandManagement()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select SubBrandId, SubBrandName, BrandName, B.Status " +
                    " from SubBrand B JOIN Brand C ON B.BrandId = C.BrandId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            SubBrandId = reader["SubBrandId"].ToString(),
                            SubBrandName = reader["SubBrandName"].ToString(),
                            BrandName = reader["BrandName"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Blog> FindBlog(string kw_submit)
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM blog" +
                    "where Title LIKE %" + "@kw_submit" +"%";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("kw_submit", kw_submit);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<Product> FindProduct(string kw_submit)
        {
            List<Product> list = new List<Product>();
            
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM product" +
                   "where ProductName LIKE % || @kw_submit || % ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("kw_submit", kw_submit);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Product()
                        {
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Blog> getAllBlog()
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog where Status = 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Blog> getListBlog()
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog " +
                             "where Status = 1 " +
                             "ORDER BY DatePost DESC ";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            DateCreate = (DateTime)reader["DateCreate"],
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Blog> get2Blog()
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog where Status = 1 ORDER BY DatePost DESC LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            DatePost = (DateTime)reader["DatePost"],
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Blog> getBlog()
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog where Status = 1 ORDER BY DatePost DESC LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            DatePost = (DateTime)reader["DatePost"],
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<BannerSlider> getAllBannerSlider()
        {
            List<BannerSlider> list = new List<BannerSlider>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from BannerSlider";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new BannerSlider()
                        {
                            SliderId = Convert.ToInt32(reader["SliderId"]),
                            SliderImage = reader["SliderImage"].ToString(),
                            SliderName = reader["SliderName"].ToString(),
                            SliderStatus = Convert.ToInt32(reader["SliderStatus"]),
                            //BlogId = Convert.ToInt32(reader["BlogId"])
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getAllProduct()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM (Product P JOIN category C ON P.CategoryId = C.CategoryId) JOIN brand B ON B.BrandId = P.BrandId;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        { 
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            BrandName = reader["BrandName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Sold = Convert.ToInt32(reader["Sold"]),
                            Cost = Convert.ToInt32(reader["Cost"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString()
                        };
                        list.Add(obj);
                }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<object> get3Product()
        {
            List<object> products = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT SUM(OrderQuantity) AS NumberSolded , ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "FROM (`product` P JOIN `orderdetail` OD ON P.ProductId = OD.ProductId) " +
                    "JOIN `order` O ON O.OrderId = OD.OrderId " +
                    "WHERE OrderStatus <> 'Đã hủy' " +
                    
                    "GROUP BY ProductName, ProductImage, P.ProductId, StartsAt, Quantity, Cost, Price " +
                    "ORDER BY SUM(OrderQuantity) DESC " +
                    "LIMIT 3;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Cost = Convert.ToInt32(reader["Cost"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            NumberSolded = Convert.ToInt32(reader["NumberSolded"])
                        };
                        products.Add(obj);
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return products;
        }
        public List<Product> getTop3ProductView()
        {
            List<Product> products = new List<Product>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM PRODUCT ORDER BY VIEW DESC LIMIT 3";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("1");
                        products.Add(new Product()
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            StartsAt = (DateTime)reader["StartsAt"],
                            Price = Convert.ToInt32(reader["Price"]),
                            View = Convert.ToInt32(reader["View"]),
                            Quantity = Convert.ToInt32(reader["Quantity"])
                        });
                    }
                }
            }
            return products;
        }

        public List<object> getLTProduct()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM Product P JOIN category C ON P.CategoryId = C.CategoryId WHERE P.CategoryId = 'LT000' AND P.Status = 1 ORDER BY CreatedAt DESC LIMIT 12 ;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getPCProduct()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM Product P JOIN category C ON P.CategoryId = C.CategoryId WHERE P.CategoryId = 'PC000' AND P.Status =1 ORDER BY CreatedAt DESC LIMIT 12 ;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getBrandProduct(int brandId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM (Product P JOIN category C ON P.CategoryId = C.CategoryId) " +
                    "JOIN brand B ON B.BrandId = P.BrandId " +
                    "JOIN subbrand S ON S.SubBrandId = P.SubBrandId " +
                    "where P.BrandId = @brandId"; 
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("BrandId", brandId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<SubBrand> getSubBrand(int brandId)
        {
            List<SubBrand> list = new List<SubBrand>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from SubBrand where BrandId = @brandId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("BrandId", brandId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        list.Add(new SubBrand()
                        {
                            SubBrandId = reader["SubBrandId"].ToString(),
                            SubBrandName = reader["SubBrandName"].ToString(),
                            BrandId = Convert.ToInt32(reader["BrandId"])
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<Brand> getBrand(int brandId)
        {
            List<Brand> list = new List<Brand>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from Brand where BrandId = @brandId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("BrandId", brandId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        list.Add(new Brand()
                        {
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            BrandName = reader["BrandName"].ToString(),
                            Description = reader["Description"].ToString(),
                            CategoryId = reader["CategoryId"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                            BrandLogo = reader["BrandLogo"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        
        public List<object> getPKProduct()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM Product P JOIN category C ON P.CategoryId = C.CategoryId WHERE P.CategoryId = 'PK000' AND P.Status = 1 ORDER BY CreatedAt DESC LIMIT 12 ;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getCateProduct(string categoryId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM (Product P JOIN category C ON P.CategoryId = C.CategoryId) WHERE AND P.Status =1 " +
                    "JOIN brand B ON B.BrandId = P.BrandId " +
                    "JOIN subbrand S ON S.SubBrandId = P.SubBrandId " +
                    "where P.CategoryId = @categoryId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("CategoryId", categoryId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

       
        public List<object> getSubProduct(string subbrandId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM (Product P JOIN category C ON P.CategoryId = C.CategoryId) " +
                    "JOIN brand B ON B.BrandId = P.BrandId " +
                    "JOIN subbrand S ON S.SubBrandId = P.SubBrandId " +
                    "where P.subbrandId = @subbrandId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("SubbrandId", subbrandId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                            Discount = Convert.ToDouble(reader["Discount"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Brand> getSub(string subbrandId)
        {
            List<Brand> list = new List<Brand>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from Brand B JOIN subbrand S ON S.BrandId = B.BrandId  where S.SubBrandId = @subbrandId LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("SubBrandId", subbrandId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        list.Add(new Brand()
                        {
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            BrandName = reader["BrandName"].ToString(),
                            Description = reader["Description"].ToString(),
                            CategoryId = reader["CategoryId"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                            BrandLogo = reader["BrandLogo"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<Brand> getBrand(string brandName)
        {
            List<Brand> list = new List<Brand>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from Brand where BrandName = @brandName LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("BrandName", brandName);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        list.Add(new Brand()
                        {
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            BrandName = reader["BrandName"].ToString(),
                            Description = reader["Description"].ToString(),
                            CategoryId = reader["CategoryId"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                            BrandLogo = reader["BrandLogo"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public List<Category> getCate(string categoryId)
        {
            List<Category> list = new List<Category>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from category where CategoryId = @categoryId LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("CategoryId", categoryId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        list.Add(new Category()
                        {
                            CategoryId = reader["CategoryId"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            Status = Convert.ToInt32(reader["Status"]),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }
        public void updateProductStatus(int productId, int status)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "UPDATE PRODUCT SET Status = @status WHERE ProductId = @productId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("status", status);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();
            }
        }

        public void updateCateStatus(string categoryId, int status)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "UPDATE Category SET Status = @status WHERE CategoryId = @categoryId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("status", status);
                cmd.Parameters.AddWithValue("categoryId", categoryId);
                cmd.ExecuteNonQuery();
            }
        }

        public void updateBrandStatus(int brandId, int status)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "UPDATE Brand SET Status = @status WHERE BrandId = @brandId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("status", status);
                cmd.Parameters.AddWithValue("brandId", brandId);
                cmd.ExecuteNonQuery();
            }
        }
        public void deleteProduct(int productId)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "DELETE FROM OrderDetail WHERE ProductId = @productId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM Comment WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM WishList WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM ProductGallary WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();

                str = "DELETE FROM Product WHERE ProductId = @productId";
                cmd = new MySqlCommand(str, conn);

                cmd.Parameters.AddWithValue("productId", productId);
                cmd.ExecuteNonQuery();
            }
        }

        public Product getProductInfo(int productId)
        {
            Product productInfo = new Product();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM PRODUCT WHERE ProductId = @productId";
                    //"SELECT * FROM (Product P JOIN category C ON P.CategoryId = C.CategoryId) " +
                    //"JOIN brand B ON B.BrandId = P.BrandId " +
                    //"JOIN subbrand S ON S.SubBrandId = P.SubBrandId " +
                    //"where ProductId = @productId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("ProductId", productId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        productInfo.ProductId = Convert.ToInt32(reader["ProductId"]);
                        productInfo.Quantity = Convert.ToInt32(reader["Quantity"]);
                        productInfo.ProductName = reader["ProductName"].ToString();
                        productInfo.ProductImage = reader["ProductImage"].ToString();
                        productInfo.Sold = Convert.ToInt32(reader["Sold"]);
                        productInfo.Cost = Convert.ToInt32(reader["Cost"]);
                        productInfo.Price = Convert.ToInt32(reader["Price"]);
                        productInfo.Status = Convert.ToInt32(reader["Status"]);
                        productInfo.Discount = Convert.ToInt32(reader["Discount"]);
                        productInfo.CategoryId = reader["CategoryId"].ToString();
                        productInfo.SubBrandId = reader["SubBrandId"].ToString();
                        if (reader["BrandId"] != DBNull.Value)
                            productInfo.BrandId = Convert.ToInt32(reader["BrandId"]);
                        productInfo.Content = reader["Content"].ToString();
                    }
                    else
                        return null;
                }
            }
            return productInfo;
        }
        public object getProductDetail(int productId)
        {
            object productInfo = new object();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM (Product P JOIN category C ON P.CategoryId = C.CategoryId) " +
                    "JOIN brand B ON B.BrandId = P.BrandId " +
                    "JOIN subbrand S ON S.SubBrandId = P.SubBrandId " +
                    "where ProductId = @productId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("ProductId", productId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("hI: " + reader["CategoryName"].ToString());
                        productInfo = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            Sold = Convert.ToInt32(reader["Sold"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Status = Convert.ToInt32(reader["Status"]),
                            Discount = Convert.ToInt32(reader["Discount"]),
                            CategoryId = reader["CategoryId"].ToString(),
                            SubBrandId = reader["SubBrandId"].ToString(),
                            BrandId = Convert.ToInt32(reader["BrandId"]),
                            Content = reader["Content"].ToString(),
                            CategoryName = reader["CategoryName"].ToString(),
                            BrandName = reader["BrandName"].ToString(),
                            SubBrandName = reader["SubBrandName"].ToString(),
                            View = Convert.ToInt32(reader["View"]),
                        };
                        
                    }
                    reader.Close();

                }
                conn.Close();

            }
            return productInfo;
        }

        public List<object> getAllOrder()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT* FROM `ORDER` O JOIN USER U ON O.UserId = U.UserId ORDER BY O.ORDERID DESC";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            OrderStatus = reader["OrderStatus"].ToString(),
                            Total = Convert.ToInt32(reader["Total"]),
                            PaymentStatus = reader["PaymentStatus"].ToString(),
                            OrderDate = (DateTime)reader["OrderDate"]
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getAllComments()
        {
            List<object> list= new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM (COMMENT C " +
                    "JOIN USER U ON C.UserId = U.UserId) " +
                    "JOIN PRODUCT P ON P.ProductId = C.ProductId " +
                    "WHERE ParentComment IS NULL " +
                    "ORDER BY Reply ASC, CommentId DESC;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            CommentId = Convert.ToInt32(reader["CommentId"]),
                            CommentContent = reader["CommentContent"].ToString(),
                            UserName = reader["LastName"].ToString() + " " + reader["FirstName"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            CreatedAt = ((DateTime)reader["CreatedAt"]).ToString("dd-MM-yyyy"),
                            Reply = Convert.ToInt32(reader["Reply"]),
                            CommentStatus = Convert.ToInt32(reader["CommentStatus"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public List<object> getSliderForHomePage()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM bannerslider BS JOIN BLOG B ON BS.BlogId = B.BlogId WHERE SliderStatus = 1 ORDER BY CreatedAt ASC LIMIT 8";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("bid: " + Convert.ToInt32(reader["BlogId"]));
                        var obj = new
                        {
                            SliderId = Convert.ToInt32(reader["SliderId"]),
                            SliderName = reader["SliderName"].ToString(),
                            SliderImage = reader["SliderImage"].ToString(),
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public Blog getBlogDetail(int blogId)
        {
            Blog Info = new Blog();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM Blog " +
                    "where BlogId = @blogId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("BlogId", blogId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                            Info.BlogId = Convert.ToInt32(reader["BlogId"]);
                            Info.Author = reader["Author"].ToString();
                            Info.Title = reader["Title"].ToString();
                            Info.Summary = reader["Summary"].ToString();
                            Info.Content = reader["Content"].ToString();
                            Info.DatePost = (DateTime)reader["DatePost"];
                        Info.View = Convert.ToInt32(reader["View"]);
                        Info.Image = reader["Image"].ToString();
                    }
                    else
                        return null;
                }
            }
            return Info;
        }

        public User getProfile(int customerId)
        {
            User Info = new User();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "SELECT * FROM user " +
                    "where UserId = @UserId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("UserId", customerId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Info.UserId = Convert.ToInt32(reader["UserId"]);
                        Info.Email = reader["Email"].ToString();
                        Info.FirstName= reader["FirstName"].ToString();
                        Info.LastName = reader["LastName"].ToString();
                        Info.Mobile = reader["Mobile"].ToString();
                        Info.UserImage = reader["UserImage"].ToString();
                    }
                    else
                        return null;
                }
            }
            return Info;
        }
        public List<Blog> getBlogRelate(int blogId)
        {
            List<Blog> list = new List<Blog>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "select * from blog where Status = 1 and BlogId <> @blogId  ORDER BY DatePost DESC LIMIT 5";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("BlogId", blogId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Blog()
                        {
                            BlogId = Convert.ToInt32(reader["BlogId"]),
                            Author = reader["Author"].ToString(),
                            Title = reader["Title"].ToString(),
                            Summary = reader["Summary"].ToString(),
                            Content = reader["Content"].ToString(),
                            DatePost = (DateTime)reader["DatePost"],
                            Image = reader["Image"].ToString(),
                        });
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public object getDefaultShippingAddress(int userId)
        {
            object item = new object();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT ShippingAddressId, Address, Phone, TT.name, QH.name, XP.name, ReceiverName, ShippingAddressType, ExtraShippingFee
                        FROM shippingaddress SA, devvn_quanhuyen QH, devvn_tinhthanhpho TT, devvn_xaphuongthitran XP
                        WHERE SA.matp = TT.matp
                        AND SA.maqh = QH.maqh
                        AND SA.xaid = XP.xaid
                        AND UserId = @UserId
                        AND IsDefault = 1;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        item = new
                        {
                            ShippingAddressId = Convert.ToInt32(reader[0].ToString()),
                            Address = reader[1].ToString(),
                            Phone = reader[2].ToString(),
                            ThanhPho = reader[3].ToString(),
                            QuanHuyen = reader[4].ToString(),
                            XaPhuong = reader[5].ToString(),
                            ReceiverName = reader[6].ToString(),
                            ShippingAddressType = reader[7].ToString(),
                            ExtraShippingFee = Convert.ToInt32(reader[8].ToString()),
                        };
                        return item;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return null;
        }

        public object getShippingAddress(int shippingAddressId)
        {
            object item = new object();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT ShippingAddressId, Address, Phone, TT.name, QH.name, XP.name, ReceiverName, ShippingAddressType, ExtraShippingFee
                        FROM shippingaddress SA, devvn_quanhuyen QH, devvn_tinhthanhpho TT, devvn_xaphuongthitran XP
                        WHERE SA.matp = TT.matp
                        AND SA.maqh = QH.maqh
                        AND SA.xaid = XP.xaid
                        AND ShippingAddressId = @ShippingAddressId;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("ShippingAddressId", shippingAddressId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        item = new
                        {
                            ShippingAddressId = Convert.ToInt32(reader[0].ToString()),
                            Address = reader[1].ToString(),
                            Phone = reader[2].ToString(),
                            ThanhPho = reader[3].ToString(),
                            QuanHuyen = reader[4].ToString(),
                            XaPhuong = reader[5].ToString(),
                            ReceiverName = reader[6].ToString(),
                            ShippingAddressType = reader[7].ToString(),
                            ExtraShippingFee = Convert.ToInt32(reader[8].ToString()),
                        };
                        return item;
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return null;
        }

        public List<object> getShippingAddressOfCustomer(int userId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT ShippingAddressId, Address, Phone, TT.name, QH.name, XP.name, ReceiverName, ShippingAddressType
                        FROM shippingaddress SA, devvn_quanhuyen QH, devvn_tinhthanhpho TT, devvn_xaphuongthitran XP
                        WHERE SA.matp = TT.matp
                        AND SA.maqh = QH.maqh
                        AND SA.xaid = XP.xaid
                        AND IsDefault = 0
                        AND UserId = @UserId;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("UserId", userId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //System.Diagnostics.Debug.WriteLine("hI: " + reader["CategoryName"].ToString());
                        var obj = new
                        {
                            ShippingAddressId = Convert.ToInt32(reader[0].ToString()),
                            Address = reader[1].ToString(),
                            Phone = reader[2].ToString(),
                            ThanhPho = reader[3].ToString(),
                            QuanHuyen = reader[4].ToString(),
                            XaPhuong = reader[5].ToString(),
                            ReceiverName = reader[6].ToString(),
                            ShippingAddressType = reader[7].ToString(),
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }
            }
            return list;
        }

        public List<object> getOrderDetail(int orderId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT * 
                            FROM OrderDetail OD join Product P ON P.ProductId = OD.ProductId 
                            WHERE OrderId = @orderid";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("orderid", orderId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            UnitPrice = Convert.ToInt32(reader["UnitPrice"]),
                            OrderQuantity = Convert.ToInt32(reader["OrderQuantity"]),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }
        public List<object> getWishList(int customerId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = "SELECT * FROM WISHLIST W JOIN PRODUCT P ON P.ProductId = W.ProductId WHERE UserId = @userId";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("userId", customerId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductImage = reader["ProductImage"].ToString(),
                            ProductName = reader["ProductName"].ToString(),
                            Price = Convert.ToInt32(reader["Price"]),
                        };
                        list.Add(obj);
                    }
                    reader.Close();
                }

                conn.Close();

            }
            return list;
        }

        public List<object> getCommentParentCommentForProductDetail(int productId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT CommentId, ProductId, C.UserId, UserImage, LastName, FirstName, CommentContent, Admin, C.CreatedAt
                            FROM COMMENT C JOIN user P ON C.UserId = P.UserId 
                            WHERE ProductId = @productid
                            AND ParentComment IS NULL 
                            AND CommentStatus = 1 
                            ORDER BY CommentId DESC";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productid", productId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            CommentId = Convert.ToInt32(reader[0]),
                            ProductId = Convert.ToInt32(reader[1]),
                            UserId = Convert.ToInt32(reader[2]),
                            UserImage = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            FirstName = reader[5].ToString(),
                            CommentContent = reader[6].ToString(),
                            Admin = Convert.ToInt32(reader[7]),
                            CreatedAt = (DateTime)reader[8],
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public List<object> getSubCommentForProductDetail(int commendId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT CommentId, ProductId, C.UserId, UserImage, LastName, FirstName, CommentContent, Admin, C.CreatedAt
                            FROM COMMENT C JOIN user P ON C.UserId = P.UserId 
                            AND ParentComment = @parentcomment
                            ORDER BY CommentId DESC";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("parentcomment", commendId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            CommentId = Convert.ToInt32(reader[0]),
                            ProductId = Convert.ToInt32(reader[1]),
                            UserId = Convert.ToInt32(reader[2]),
                            UserImage = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            FirstName = reader[5].ToString(),
                            CommentContent = reader[6].ToString(),
                            Admin = Convert.ToInt32(reader[7]),
                            CreatedAt = (DateTime)reader[8],
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public List<Product> getSanPhamKhongBanDuoc(DateTime tu_ngay, DateTime den_ngay)
        {
            List<Product> list = new List<Product>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT * FROM `product`
                        WHERE ProductId NOT IN (    SELECT ProductId
                        	                        FROM `order` O JOIN `orderdetail` OD ON O.OrderId = OD.OrderId
                                                    WHERE OrderDate BETWEEN @startdate AND @enddate)";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", tu_ngay);
                cmd.Parameters.AddWithValue("enddate", den_ngay);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var product = new Product()
                        {
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Cost = Convert.ToInt32(reader["Cost"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductImage = reader["ProductImage"].ToString(),
                            Sold = Convert.ToInt32(reader["Sold"]),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                        };
                        list.Add(product);
                    }
                }
            }
            return list;
        }

        public List<object> getCustomerRevenueStatistic(DateTime tu_ngay, DateTime den_ngay)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT U.UserId, FirstName, LastName, SUM(OrderQuantity * (Price - Cost)) as Revenue, COUNT(DISTINCT O.OrderId) as NumberOrder
                            FROM ((`user` U JOIN `order` O ON O.UserId = U.UserId)
		                            JOIN `orderdetail` OD ON OD.OrderId = O.OrderId)
        	                            JOIN `product` P ON P.ProductId = OD.ProductId
                            WHERE Admin = 0
                            AND OrderDate BETWEEN @startdate AND @enddate
                            GROUP BY U.UserId, FirstName, LastName;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", tu_ngay);
                cmd.Parameters.AddWithValue("enddate", den_ngay);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            Revenue = Convert.ToInt32(reader["Revenue"]),
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            NumberOrder = Convert.ToInt32(reader["NumberOrder"]),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public List<object> getCustomerLoginStatistic(DateTime tu_ngay, DateTime den_ngay)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT COUNT(*) AS NumberAccess, FirstName, LastName
                            FROM `loginhistory` L JOIN `user` U ON U.UserId = L.UserId
                            WHERE Admin = 0 
                            AND LOGINDATE BETWEEN @startdate AND @enddate 
                            GROUP BY FirstName, LastName";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", tu_ngay);
                cmd.Parameters.AddWithValue("enddate", den_ngay);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            NumberAccess = Convert.ToInt32(reader["numberAccess"]),
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public List<object> getKhachHangTiemNang(DateTime tu_ngay, DateTime den_ngay)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT COUNT(*) AS NumberAccess, FirstName, LastName
                            FROM `loginhistory` L JOIN `user` U ON U.UserId = L.UserId
                            WHERE Admin = 0 
                            AND LOGINDATE BETWEEN @startdate AND @enddate 
                            GROUP BY FirstName, LastName
                            ORDER BY NumberAccess DESC
                            LIMIT 5;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("startdate", tu_ngay);
                cmd.Parameters.AddWithValue("enddate", den_ngay);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            NumberAccess = Convert.ToInt32(reader["numberAccess"]),
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public object getOrderInfo(int OrderId)
        {
            object orderInfo = new object();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT U.UserId as UserId, OrderId, LastName, FirstName, OrderDate, OrderStatus, Email, Mobile, ShipFee, PaymentMethod, Total, PaymentStatus, ShipMethod
                        FROM `order` O JOIN `user` U ON U.UserId = O.UserId 
                        WHERE OrderId = @orderid";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("orderid", OrderId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        orderInfo = new
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            OrderId = Convert.ToInt32(reader["OrderId"]),
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            OrderDate = (DateTime)reader["OrderDate"],
                            OrderStatus = reader["OrderStatus"].ToString(),
                            Email = reader["Email"].ToString(),
                            Mobile = reader["Mobile"].ToString(),
                            ShipFee = Convert.ToInt32(reader["ShipFee"]),
                            PaymentMethod = reader["PaymentMethod"].ToString(),
                            Total = Convert.ToInt32(reader["Total"]),
                            PaymentStatus = reader["PaymentStatus"].ToString(),
                            ShipMethod = reader["ShipMethod"].ToString(),
                        };
                    }
                }
            }
            return orderInfo;
        }
        public List<object> getRating(int ProductId)
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT U.UserId as UserId, FirstName, LastName, Title, Content, Rating, CreatedAt, UserImage, ProductRatingStatus
                            FROM `productrating` PR JOIN `user` U ON PR.UserId = U.UserId 
                            WHERE PR.ProductId = @productid
                            AND ProductRatingStatus = 1";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("productid", ProductId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            UserImage = reader["UserImage"].ToString(),
                            Title = reader["Title"].ToString(),
                            Content = reader["Content"].ToString(),
                            Rating = Convert.ToInt32(reader["Rating"]),
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ProductRatingStatus = reader["ProductRatingStatus"].ToString(),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public List<object> getAllRating()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT U.UserId as UserId, FirstName, LastName, Title, PR.Content, Rating, PR.CreatedAt, UserImage, ProductName, PR.ProductId as ProductId, ProductRatingStatus
                            FROM (`productrating` PR JOIN `user` U ON PR.UserId = U.UserId )
                                JOIN `product` P ON P.ProductId = PR.ProductId
                            ORDER BY DATE(PR.CreatedAt) DESC";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            LastName = reader["LastName"].ToString(),
                            FirstName = reader["FirstName"].ToString(),
                            UserImage = reader["UserImage"].ToString(),
                            Title = reader["Title"].ToString(),
                            Content = reader["Content"].ToString(),
                            Rating = Convert.ToInt32(reader["Rating"]),
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            ProductRatingStatus = Convert.ToInt32(reader["ProductRatingStatus"]),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }
        public List<object> getGiamGiaSoc()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT AVG(Rating) AS avg_rating,  ProductImage, ProductName, P.ProductId , Price, Cost, Discount, View, Sold, StartsAt, Quantity
                            FROM `productrating` PR JOIN `product` P ON P.ProductId = PR.ProductId
                            WHERE Quantity > 1 AND Status = 1 AND DISCOUNT > 0 
                            group by ProductImage, ProductName, P.ProductId, Price, Cost, Discount, View, Sold, StartsAt
                            LIMIT 4;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            ProductImage = reader["ProductImage"].ToString(),
                            Quantity =  Convert.ToInt32(reader["Quantity"]),
                            //Title = reader["Title"].ToString(),
                            //Content = reader["Content"].ToString(),
                            AvgRating = Convert.ToInt32(reader["avg_rating"]),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            //ProductRatingStatus = Convert.ToInt32(reader["ProductRatingStatus"]),
                            Price = Convert.ToInt32(reader["Price"]),
                            Cost = Convert.ToInt32(reader["Cost"]),
                            Discount = Convert.ToInt32(reader["Discount"]),
                            View = Convert.ToInt32(reader["View"]),
                            Sold = Convert.ToInt32(reader["Sold"]),
                            StartsAt = (DateTime)reader["StartsAt"],
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }
        public List<object> getAllExtraShipfee()
        {
            List<object> list = new List<object>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = @"SELECT QH.name as quanhuyen , TP.name as tinhtp, maqh, ExtraShippingFee 
                            FROM devvn_quanhuyen QH JOIN devvn_tinhthanhpho TP ON QH.matp = TP.matp;";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var obj = new
                        {
                            quanhuyen = reader["quanhuyen"].ToString(),
                            maqh = reader["maqh"].ToString(),
                            ExtraShippingFee = Convert.ToInt32(reader["ExtraShippingFee"]),
                            tinhtp = reader["tinhtp"].ToString(),
                        };
                        list.Add(obj);
                    }
                }
            }
            return list;
        }
        /*================Code thầy Hùng =================*/
        public Product findProduct(int Id)
        {
            Product pro = new Product();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "select * from Product where ProductId=@ma";
                MySqlCommand cmd = new MySqlCommand(str, conn);
                cmd.Parameters.AddWithValue("ma", Id);
                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    pro.ProductId = Convert.ToInt32(reader["ProductId"]);
                    pro.ProductName = reader["ProductName"].ToString();
                    pro.Price = Convert.ToInt32(reader["Price"].ToString());
                    pro.ProductImage = reader["ProductImage"].ToString();
                }
            }
            return (pro);
        }
        /*================Hết code thầy Hùng =================*/
    }
}
