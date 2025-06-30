using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OHTAlgorithm
{
    public class CompleteTest
    {
        public static void Main()
        {
            var calculator = new OHTRouteCalculator();
            
            // 使用您提供的完整 JSON 佈局資料
            string jsonLayout = GetCompleteLayoutJson();
            
            try
            {
                calculator.LoadLayout(jsonLayout);
                Console.WriteLine("佈局載入成功！");
                Console.WriteLine("🌟 使用 A* 演算法進行路徑規劃（基於歐幾里得距離啟發式）");
                
                // 顯示可用的節點
                ShowAvailableNodes(calculator);
                
                while (true)
                {
                    Console.WriteLine("\n=== OHT 路徑規劃系統 ===");
                    Console.WriteLine("請輸入起點 ID (或輸入 'exit' 退出): ");
                    string? startId = Console.ReadLine();
                    
                    if (string.IsNullOrEmpty(startId) || startId.ToLower() == "exit")
                    {
                        break;
                    }
                    
                    Console.WriteLine("請輸入終點 ID: ");
                    string? endId = Console.ReadLine();
                    
                    if (string.IsNullOrEmpty(endId))
                    {
                        Console.WriteLine("終點 ID 不能為空！");
                        continue;
                    }
                    
                    TestRoute(calculator, startId, endId, $"從 {startId} 到 {endId}");
                }
                
                Console.WriteLine("\n感謝使用！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"錯誤: {ex.Message}");
            }
        }
        
        private static void ShowAvailableNodes(OHTRouteCalculator calculator)
        {
            Console.WriteLine("\n🏗️  可用的節點：");
            Console.WriteLine("=== 🚉 車站 (STATION) ===");
            Console.WriteLine("H1, H2, H3, H4, H5, H6, H7, H8, H9, H10, H11");
            Console.WriteLine("V1, V2, V3, V4");
            
            Console.WriteLine("\n=== 🔀 開關 (SWITCH) ===");
            Console.WriteLine("D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11");
            Console.WriteLine("C1-L, C1-R, C2-L, C2-R, C3-L, C3-R");
            Console.WriteLine("C4-L, C4-R, C5-L, C5-R, C6-L, C6-R");
            Console.WriteLine("C7-L, C7-R, C8-L, C8-R, C12-L, C12-R, C13-L, C13-R");
            
            Console.WriteLine("\n=== ⭕ 中繼點 (VIA_POINT) ===");
            Console.WriteLine("B1, B2, B3");
            
            Console.WriteLine("\n💡 提示：請輸入任意兩個節點 ID 來計算路徑");
        }
        
        private static void TestRoute(OHTRouteCalculator calculator, string start, string end, string description)
        {
            try
            {
                Console.WriteLine($"\n=== {description} ===");
                var route = calculator.RouteOHT(start, end, "main_layout");
                
                Console.WriteLine($"✅ 找到路徑，共 {route.Count} 個步驟：");
                foreach (var step in route)
                {
                    string typeInfo = !string.IsNullOrEmpty(step.Type) ? $"{step.Type} " : "";
                    Console.WriteLine($"  步驟 {step.No}: {typeInfo}{step.Id} (側邊: {step.Side})");
                }
                
                // 詢問是否顯示 JSON 格式
                Console.WriteLine("\n選擇顯示格式:");
                Console.WriteLine("1. 完整 JSON (包含 type 和 switch-option)");
                Console.WriteLine("2. 簡化 JSON (只包含 no, id, side)");
                Console.WriteLine("3. 不顯示");
                Console.Write("請選擇 (1/2/3): ");
                string? choice = Console.ReadLine();
                
                if (choice == "1")
                {
                    Console.WriteLine("\n完整 JSON 格式:");
                    string jsonResult = JsonConvert.SerializeObject(route, Formatting.Indented);
                    Console.WriteLine(jsonResult);
                }
                else if (choice == "2")
                {
                    Console.WriteLine("\n簡化 JSON 格式:");
                    var simpleRoute = calculator.RouteOHTSimple(start, end, "main_layout");
                    string jsonResult = JsonConvert.SerializeObject(simpleRoute, Formatting.Indented);
                    Console.WriteLine(jsonResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 路徑計算失敗: {ex.Message}");
                
                if (ex.Message.Contains("not found"))
                {
                    Console.WriteLine("請檢查節點 ID 是否正確。");
                }
            }
        }
        
        private static string GetCompleteLayoutJson()
        {
            // 使用您提供的完整 JSON 佈局資料
            return @"{
    ""nodes"": [
        {
            ""id"": ""D1"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 41,
                ""y"": 10
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""B1"",
            ""type"": ""VIA_POINT"",
            ""position"": {
                ""x"": 11,
                ""y"": 10
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""B2"",
            ""type"": ""VIA_POINT"",
            ""position"": {
                ""x"": 11,
                ""y"": 81
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""V1"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 11,
                ""y"": 47
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H1"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 25,
                ""y"": 10
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H2"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 25,
                ""y"": 82
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""V2"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 41,
                ""y"": 36
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""V3"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 41,
                ""y"": 57
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""V4"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 179,
                ""y"": 57
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""D2"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 41,
                ""y"": 25
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D3"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 41,
                ""y"": 47
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D4"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 41,
                ""y"": 69
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C1-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 41,
                ""y"": 78
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C1-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 38,
                ""y"": 82
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""H3"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 94,
                ""y"": 70
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H4"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 94,
                ""y"": 47
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H5"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 94,
                ""y"": 26
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H6"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 169,
                ""y"": 26
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H7"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 94,
                ""y"": 10
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""C2-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 71,
                ""y"": 82
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C2-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 74,
                ""y"": 86
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D5"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 80,
                ""y"": 81
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C3-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 158,
                ""y"": 81
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C3-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 158,
                ""y"": 89
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D6"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 174,
                ""y"": 82
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C4-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 178,
                ""y"": 73
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C4-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 174,
                ""y"": 69
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C5-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 179,
                ""y"": 50
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C5-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 173,
                ""y"": 47
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C6-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 176,
                ""y"": 26
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C6-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 179,
                ""y"": 31
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D7"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 185,
                ""y"": 26
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D8"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 213,
                ""y"": 25
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D9"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 153,
                ""y"": 10
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D10"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 191,
                ""y"": 47
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""D11"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 191,
                ""y"": 68
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C7-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 213,
                ""y"": 14
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C7-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 217,
                ""y"": 10
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C8-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 152,
                ""y"": 22
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C8-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 146,
                ""y"": 26
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""H8"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 234,
                ""y"": 68
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H9"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 233,
                ""y"": 48
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H10"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 234,
                ""y"": 26
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""H11"",
            ""type"": ""STATION"",
            ""position"": {
                ""x"": 234,
                ""y"": 10
            },
            ""switch_options"": {}
        },
        {
            ""id"": ""C12-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 252,
                ""y"": 26
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C12-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 259,
                ""y"": 30
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C13-L"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 258,
                ""y"": 14
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""C13-R"",
            ""type"": ""SWITCH"",
            ""position"": {
                ""x"": 263,
                ""y"": 10
            },
            ""switch_options"": {
                ""left"": """",
                ""right"": """"
            }
        },
        {
            ""id"": ""B3"",
            ""type"": ""VIA_POINT"",
            ""position"": {
                ""x"": 323,
                ""y"": 10
            },
            ""switch_options"": {}
        }
    ],
    ""edges"": [
        {
            ""from"": ""D1"",
            ""to"": ""H1"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""H1"",
            ""to"": ""B1"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""B1"",
            ""to"": ""V1"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""V1"",
            ""to"": ""B2"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""B2"",
            ""to"": ""H2"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""H2"",
            ""to"": ""C1-R"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""C1-R"",
            ""to"": ""C2-L"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""C1-L"",
            ""to"": ""C2-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D1"",
            ""to"": ""D2"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D2"",
            ""to"": ""V2"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""V2"",
            ""to"": ""D3"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D3"",
            ""to"": ""V3"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""V3"",
            ""to"": ""D4"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D4"",
            ""to"": ""C1-L"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D3"",
            ""to"": ""H4"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D4"",
            ""to"": ""H3"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""H5"",
            ""to"": ""C8-R"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""C8-R"",
            ""to"": ""H6"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""C8-L"",
            ""to"": ""H6"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D9"",
            ""to"": ""C8-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D9"",
            ""to"": ""H7"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""H7"",
            ""to"": ""D1"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""H6"",
            ""to"": ""C6-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C6-L"",
            ""to"": ""D7"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C6-R"",
            ""to"": ""D7"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""H4"",
            ""to"": ""C5-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""H3"",
            ""to"": ""C4-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C4-L"",
            ""to"": ""V4"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C4-R"",
            ""to"": ""V4"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""C2-L"",
            ""to"": ""D5"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C2-R"",
            ""to"": ""D5"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D5"",
            ""to"": ""C3-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C3-L"",
            ""to"": ""D6"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C3-R"",
            ""to"": ""D6"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D6"",
            ""to"": ""C4-R"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""V4"",
            ""to"": ""C5-R"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""C5-R"",
            ""to"": ""C6-R"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""C5-L"",
            ""to"": ""C6-R"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D7"",
            ""to"": ""D8"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D7"",
            ""to"": ""D10"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D10"",
            ""to"": ""D11"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D8"",
            ""to"": ""C7-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C7-L"",
            ""to"": ""D9"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C7-R"",
            ""to"": ""D9"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D11"",
            ""to"": ""H8"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D10"",
            ""to"": ""H9"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""D8"",
            ""to"": ""H10"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""H10"",
            ""to"": ""C12-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C12-L"",
            ""to"": ""C13-L"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""C13-L"",
            ""to"": ""H11"",
            ""cost"": 1,
            ""side"": ""left""
        },
        {
            ""from"": ""H11"",
            ""to"": ""C7-R"",
            ""cost"": 1,
            ""side"": ""right""
        },
        {
            ""from"": ""D2"",
            ""to"": ""H5"",
            ""cost"": 1,
            ""side"": ""left""
        }
    ]
}";
        }
    }
} 