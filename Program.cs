const string WHERETOGET = "WHERE TO GET";
const string SCHNITZEL = "SCHNITZEL";
const int APPETIZER = 0, MAINDISH = 1, DESSERT = 2, SEITANSCHNITZEL = 3;

string path = @"C:\PROG2\006-files\Schnizel Hunt\Schnizel Hunt\MenuCard\";
var datas = Directory.GetFiles(path);
var seitanSchnitzel = new List<string>();
var appetizers = new List<string>();
var dessert = new List<string>();
var mainDish = new List<string>();
bool takeAppetizers = false;
bool takeDessert = false;
bool takeMainDish = false;

SetOutputInColor($"{WHERETOGET} SCHNITZEL?");
SetOutputInColor("========================");
Console.WriteLine();

foreach (string fileName in datas)
{
    var rows = File.ReadAllLines(fileName);
    bool fileNameAlreadyOutputted = false;

    foreach (string row in rows)
    {
        if (row.Contains("APPETIZERS")) { takeAppetizers = true; takeDessert = takeMainDish = false; }
        else if (row.Contains("MAIN DISH")) { takeMainDish = true; takeDessert = takeAppetizers = false; }
        else if (row.Contains("DESSERT")) { takeDessert = true; takeMainDish = takeAppetizers = false; }

        if (row == string.Empty || row == "APPETIZERS" || row == "MAIN DISHES" || row == "DESSERTS") continue;
        else if (takeAppetizers) appetizers.Add(row);
        else if (takeDessert) dessert.Add(row);
        else if (takeMainDish) mainDish.Add(row);

        if (row.Contains("Schnitzel"))
        {
            if (row.Contains("Seitan Schnitzel")) seitanSchnitzel.Add(row);
            if (fileNameAlreadyOutputted == false) { SetOutputInColor(Path.GetFileNameWithoutExtension(fileName)); fileNameAlreadyOutputted = true; }
            Console.WriteLine("  {0}", row.Substring(0, row.IndexOf(":")));
        }
    }
}
Console.WriteLine();

var food = new[] { appetizers.ToArray(), mainDish.ToArray(), dessert.ToArray(),seitanSchnitzel.ToArray() };

CheapestAndMostExpensiveFoods(food, out var min2, datas, out var file, out string max);

SetOutputInColor($"{WHERETOGET} THE CHEAPEST SEITAN {SCHNITZEL}?");
SetOutputInColor("============================================");
Console.WriteLine("{0}, {1}", file[SEITANSCHNITZEL], ReplaceEuroSign(min2[SEITANSCHNITZEL].Split(":")[1]));
Console.WriteLine();

SetOutputInColor($"{WHERETOGET} THE MOST EXPENSIVE SEITAN {SCHNITZEL}?");
SetOutputInColor("==================================================");
Console.WriteLine("{0}, {1}", file[SEITANSCHNITZEL + 1], ReplaceEuroSign(max.Split(":")[1]));
Console.WriteLine();

SetOutputInColor($"{WHERETOGET} THE CHEAPEST {SCHNITZEL} FEAST?");
SetOutputInColor("==================================================");

Console.WriteLine("Appetizer: {0}, {1}", file[APPETIZER], ReplaceEuroSign(min2[APPETIZER]));
Console.WriteLine("Main Dish: {0}, {1}", file[MAINDISH], ReplaceEuroSign(min2[MAINDISH]));
Console.WriteLine("Dessert: {0}, {1}", file[DESSERT], ReplaceEuroSign(min2[DESSERT]));

void CheapestAndMostExpensiveFoods(string[][] food, out string[] min, string[] datas, out string[] files, out string maxSeitanSchnitzel)
{
    min = new string[4]; files = new string[5];
    min[SEITANSCHNITZEL] = maxSeitanSchnitzel = food[SEITANSCHNITZEL][0];
    min[APPETIZER] = food[APPETIZER][0]; min[MAINDISH] = food[MAINDISH][0]; min[DESSERT] = food[DESSERT][0];
    files[0] = files[1] = files[2] = files[3] = files[4] = string.Empty;

    int indexCategory = 0;

    foreach (string[] category in food)
    {
        for (int i = 0; i < category.Length; i++)
        {
            var parts = category[i].Split(":");
            var partsMin = min[indexCategory].Split(":");
            
            if (int.Parse(parts[1][..^1]) < int.Parse(partsMin[1][..^1]))
            {
                min[indexCategory] = category[i];
            }
            if (indexCategory == SEITANSCHNITZEL)
            {
                var partsMax = maxSeitanSchnitzel.Split(":");
                if (int.Parse(parts[1][..^1]) > int.Parse(partsMax[1][..^1]))
                {
                    maxSeitanSchnitzel = category[i];
                }
            }
        }

        indexCategory++;
    }

    foreach (string fileName in datas)
    {
        for (int i = 0; i < min.Length; i++)
        {
            if (File.ReadAllText(fileName).Contains(min[i])) { files[i] = Path.GetFileNameWithoutExtension(fileName); }
            else if (File.ReadAllText(fileName).Contains(maxSeitanSchnitzel)){ files[SEITANSCHNITZEL + 1] = Path.GetFileNameWithoutExtension(fileName); }
        }
    }
}
void SetOutputInColor(string input)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(input);
    Console.ResetColor();
}
string ReplaceEuroSign(string text) { return text.Replace("€", " Euro"); }