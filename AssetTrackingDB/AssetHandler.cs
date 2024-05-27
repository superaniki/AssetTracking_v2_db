using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssetTracking;
using EntityDBTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AssetTracking
{
    enum AssetType
    {
        Computer,
        Phone
    }


    public class AssetHandler
    {
        //public List<Asset> assetList;
        MyDbContext context;
        public AssetHandler()
        {
            context = new();
        }

        private void PrintMainHeader()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("          AssetTracker DELUXE - DB Edition        ");
            Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^\n\n");
        }
        public void MainMenu()
        {
            string? input = "";
            while (true)
            {
                PrintMainHeader();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Add Assets");
                Console.WriteLine("2. List Assets");
                Console.WriteLine("3. Update Assets");
                Console.WriteLine("4. Delete Assets");
                Console.WriteLine("-------------------");
                Console.WriteLine("5. Exit this boring app and go grow a carrot...!\n");
                Console.Write("> ");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddAssets();
                        break;
                    case "2":
                        var assetList = context.Assets.Include(x => x.Office).ToList();
                        PrintAssets(assetList, true);
                        PressAnyKey();
                        break;
                    case "3":
                        DeleteAssets();
                        break;
                    case "4":
                        DeleteAssets();
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine("Byebye...\n\n");
                        return;
                }
            }
        }

        public void PressAnyKey()
        {
            Console.WriteLine("\n < Press Any Key >");
            Console.ReadKey();
        }

        public static string? PromptInput()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("> ");
            Console.ResetColor();
            return Console.ReadLine();
        }


        public void DeleteAssets()
        {
            var assetList = context.Assets.Include(x => x.Office).ToList();

            while (true)
            {
                PrintAssets(assetList, true);
                Console.Write("\nWhich asset do you want to delete? (Q to quit) ");
                var numberString = PromptInput();
                if (int.TryParse(numberString, out int parsedNumber))
                {
                    if (parsedNumber > 0 && parsedNumber <= assetList.Count())
                    {
                        PrintMainHeader();
                        PrintOneAssetWithHeader(assetList[parsedNumber - 1]);
                        if (AskUserConfirmation("\n\nAre you sure you want to delete this asset?", "Deletion aborted."))
                        {
                            var asset = context.Assets.Remove(assetList[parsedNumber - 1]);
                            UpdateDatabase("Asset deleted from database!", "Error deleting asset from DB");
                            return;
                        }

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error, number not in list");
                        Console.ResetColor();
                    }
                }
                else if (numberString.ToUpper() == "Q")
                {
                    break;/*
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not a valid number");
                    Console.ResetColor();
                    */
                }
            }
        }


        public void AddAssets()
        {
            PrintMainHeader();
            Console.ForegroundColor = ConsoleColor.White;
            var assetTypes = Enum.GetValues(typeof(AssetType)).Cast<AssetType>().ToArray();

            Console.WriteLine("--- Add asset --- ");
            var assetType = "";

            while (assetType == "")
            {
                Console.Write("Select asset type: ");
                Console.ForegroundColor = ConsoleColor.Green;

                var index = 1;
                Console.Write("( ");
                foreach (var asset in assetTypes)
                {
                    Console.Write($"{index}:{asset} ");
                    index++;
                }
                Console.WriteLine(")");
                var numberString = PromptInput();

                if (int.TryParse(numberString, out int parsedNumber))
                {
                    if (parsedNumber > 0 && parsedNumber <= assetTypes.Count())
                        assetType = Enum.GetName(assetTypes[parsedNumber - 1]);
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error, not available number");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not a valid number");
                    Console.ResetColor();
                }
            }

            var brand = "";
            while (brand == "")
            {
                Console.WriteLine("\nEnter brand");
                brand = PromptInput();

            }
            var model = "";
            while (model == "")
            {
                Console.WriteLine("\nEnter model");
                model = PromptInput();
            }

            var offices = context.Offices.OrderByDescending(elem => elem.Country).ToArray();
            Office? selectedOffice = null;
            while (selectedOffice == null)
            {
                Console.Write("\nSelect Office: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("( ");
                int count = 0;
                foreach (var office in offices)
                {
                    Console.Write(count + 1 + ":" + office.Country + " ");
                    count++;
                }
                Console.WriteLine(")");

                var numberString = PromptInput();
                if (int.TryParse(numberString, out int parsedNumber))
                {
                    if (parsedNumber > 0 && parsedNumber <= offices.Count())
                        selectedOffice = offices[parsedNumber - 1];
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not a valid number");
                    Console.ResetColor();
                }
            }

            string? dateInput;
            DateTime choosenDate;
            do
            {
                Console.Write("\nEnter purchase date: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("(YYYY-MM-DD)");
                dateInput = PromptInput();

            } while (!DateTime.TryParse(dateInput, out choosenDate));


            int price = -1;
            while (price == -1)
            {
                Console.WriteLine($"\nEnter Price({selectedOffice.Currency})");
                var numberString = PromptInput();
                if (int.TryParse(numberString, out int parsedNumber))
                {
                    if (parsedNumber >= 0)
                        price = parsedNumber;
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Number must be larger than -1");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not a valid number");
                    Console.ResetColor();
                }
            }


            Asset newAsset;

            // Full name of the type including the namespace
            var fullTypeName = $"AssetTracking.{assetType}";

            // Get the type object
            var type = Type.GetType(fullTypeName);

            if (type != null && typeof(Asset).IsAssignableFrom(type))
            {
                // Create an instance of the type
                newAsset = (Asset)Activator.CreateInstance(type)!;
                //Console.WriteLine($"Created asset of type: {newAsset.GetType().Name}");

                newAsset.Model = model!;
                newAsset.Office = selectedOffice;
                newAsset.OfficeId = selectedOffice.Id;
                newAsset.Brand = brand!;
                newAsset.Price = price;
                newAsset.DateOfPurchase = choosenDate;

                Console.WriteLine("\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                PrintOneAssetWithHeader(newAsset);
                Console.ResetColor();

                if (!AskUserConfirmation("\nAdd to Database?", "Asset aborted."))
                    return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Invalid asset type");
                PressAnyKey();
                return;
            }

            // user inputted Y => Lets try saving
            context.Add(newAsset);
            UpdateDatabase("Database updated with new asset!", "Error saving asset to DB");
        }

        public void UpdateDatabase(string successMessage, string errorMessage)
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n{errorMessage} : " + e.Message);
                PressAnyKey();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{successMessage}");
            PressAnyKey();
        }

        public bool AskUserConfirmation(string questionText, string abortionText)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(questionText);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  (Y/N)");
                var input = PromptInput()?.ToUpper();
                if (input == "Y")
                {
                    return true;
                }
                else if (input == "N")
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(abortionText);
                    PressAnyKey();
                    return false;
                }
            }
        }

        public void PrintOneAssetWithHeader(Asset asset)
        {
            PrintAssetHeader();
            Console.ForegroundColor = ConsoleColor.White;
            PrintAssetDetails(asset);
        }

        public void PrintAssetHeader()
        {
            Console.Write("\tType".PadRight(15) + "Brand".PadRight(15) + "Model".PadRight(15) + "Office".PadRight(15) + "Purchase date".PadRight(18));
            Console.WriteLine("Price".PadRight(12) + "Currency");
            Console.Write("\t----".PadRight(15) + "-----".PadRight(15) + "-----".PadRight(15) + "------".PadRight(15) + "-------------".PadRight(18));
            Console.WriteLine("-----".PadRight(12) + "--------");
        }
        public void PrintAssetDetails(Asset asset)
        {
            var (brand, model, date, price, office, currency) = asset;
            var type = asset.GetType().Name;
            Console.Write($"\t{type}".PadRight(15) + $"{brand}".PadRight(15) + $"{model}".PadRight(15) + $"{office}".PadRight(15) + $"{date.ToString("MM-dd-yyyy")}".PadRight(18));
            Console.WriteLine($"{Math.Round(price, 2)}".PadRight(12) + $"{currency}");
            Console.ResetColor();
        }

        public void UpdateWarningLevels(List<Asset> assets)
        {
            DateTime today = DateTime.Today; // time expensive calculations, do only once per loop
            foreach (Asset asset in assets)
            {
                asset.UpdateWarningLevel(today);
            }
        }

        public List<Asset> GetSortedAssets(List<Asset> unsortedAssets)
        {
            List<Asset> sortedAssets = new();
            var groupedObjects = unsortedAssets.GroupBy(asset => asset.OfficeId);
            foreach (var group in groupedObjects)
            {
                var orderedGroup = group.OrderBy(asset => asset.DateOfPurchase);
                foreach (Asset asset in orderedGroup)
                {
                    sortedAssets.Add(asset);
                }
            }
            return sortedAssets;
        }

        public void SetAssetColor(Asset asset)
        {
            switch (asset.Status)
            {
                case Asset.ExpirationStatus.Overdue:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case Asset.ExpirationStatus.Status_Default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case Asset.ExpirationStatus.Warning_Level1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case Asset.ExpirationStatus.Warning_Level2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
        }

        public void PrintAssets(List<Asset> assetList, bool numbered = false)
        {
            PrintMainHeader();
            Console.ForegroundColor = ConsoleColor.Yellow;
            PrintAssetHeader();

            List<Asset> sortedAssets = GetSortedAssets(assetList);
            UpdateWarningLevels(sortedAssets);

            var index = 1;

            foreach (Asset asset in sortedAssets)
            {
                SetAssetColor(asset);
                if (numbered)
                    Console.Write($"{index}" + ". ");
                PrintAssetDetails(asset);
                index++;
            }
        }
    }
}