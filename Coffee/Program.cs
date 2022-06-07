using System;
using System.Collections.Generic;
using System.Threading;

namespace Coffee
{
    class Program
    {
        static void Main(string[] args)
        {   
            DrinkMaker drinkMaker = new DrinkMaker();
            drinkMaker.MakeDrink();
        }

      

        class DrinkMaker : Menu
        {

            public Drink Drink { get => _drink; set { _drink = value; } }
            private Drink _drink;

            public List<Drink> Drinks { get => _drinks; set { _drinks = value; } }
            private List<Drink> _drinks;

            public Drink espresso;

            public DrinkMaker()
            {
                Drink = new Drink();
                
            }

            public void MakeDrink()
            {
                
                char choice;
                do
                {

                    Drink.drinkList.Clear();
                    Console.WriteLine('\t');
                    Console.WriteLine("______________");
                    Console.WriteLine("1] Espresso");
                    Console.WriteLine("2] Americano");
                    Console.WriteLine("3] Latte");
                    Console.WriteLine("______________");
                    Console.WriteLine('\t');
                    choice = Console.ReadKey(true).KeyChar;
                    switch (choice)
                    {

                        case '1':
                            Loading();
                            CreateEspresso();
                            Show(espresso);
                            break;
                        case '2':
                            Loading();
                            CreateAmericano();
                            break;
                        case '3':
                            Loading();
                            CreateLatte();
                            break;

                    }
                } while (choice != (char)ConsoleKey.Escape);



            }
            public void AddToDrink(Ingredient item, double amount)
            {
                Drink.drinkList.Add(item);
                switch (item.Name)
                {

                    case "Coffee":
                        Console.WriteLine($"{Drink.drinkList.Count}] {item.Name}\t{amount} gr");
                        break;
                    case "Water":
                        Console.WriteLine($"{Drink.drinkList.Count}] {item.Name}\t{amount} ml");
                        break;
                    case "Milk":
                        Console.WriteLine($"{Drink.drinkList.Count}] {item.Name}\t\t{amount} ml");
                        break;
                    case "Sugar":
                        Console.WriteLine($"{Drink.drinkList.Count}] {item.Name}\t{amount} sp");
                        break;
                }

            }
                  
            public void CreateEspresso()
            {
                
                AddToDrink(Coffee, 7);
                AddToDrink(Water, 50);
                espresso = new Drink();
                Console.WriteLine(espresso.Name); 

            }
            public void CreateAmericano()
            {
                
                CreateEspresso();
                AddToDrink(Water, 50);
            }
            public void CreateLatte()
            {

                
                AddToDrink(Milk, 150);
                CreateEspresso();
                
                
                
            }



            
            public void Loading()
            {
                Console.Clear();
                Console.WriteLine("Loading.");
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("Loading..");
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("Loading...");
                Thread.Sleep(1000);
                Console.Clear();


                
                Console.WriteLine("Sugar?\n1] Yes.\n2] No.\n\n");
                var choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {
                    case '1':
                        Console.WriteLine("Spoon: ");
                        var spoon = (char)Console.ReadKey(true).KeyChar - '0';

                        if (spoon != 0)
                        {
                            Console.Clear();
                            
                        }

                        AddToDrink(Sugar, Convert.ToInt32(spoon));

                        break;
                    case '2':
                        break;
                }
            }


            public void Show(Drink drink)
            {
                Drinks.Add(drink);
                
            }
        }

        class Menu : Ingredient
        {

            public CoffeeModule Coffee { get => _coffee; set { _coffee = value; } }
            private CoffeeModule _coffee;

            public WaterModule Water { get => _water; set { _water = value; } }
            private WaterModule _water;

            public MilkModule Milk { get => _milk; set { _milk = value; } }
            private MilkModule _milk;

            public SugarModule Sugar { get => _sugar; set { _sugar = value; } }
            private SugarModule _sugar;

            public DrinkMaker DrinkMaker { get => _drinkMaker; set { _drinkMaker = value; } }
            private DrinkMaker _drinkMaker;

            
            public string Position { get => _position; set { _position = value; } }
            private string _position;

            public int Cup { get => _сup; set { _сup = value; } }
            private int _сup;

            public double Price { get => _price; set { _price = value; } }
            private double _price;

            
           

            public Menu()
            {
                Coffee = new CoffeeModule();
                Water = new WaterModule();
                Milk = new MilkModule();
                Sugar = new SugarModule();
                
                Position = "Drink";
                Cup = 250;
                Price = 00.00;

            }

            

        }

        class Ingredient 
        {
            public string Name { get => _name; set { _name = value; } }
            private string _name;
            public double Amount { get => _amount; set { _amount = value; } }
            public double _amount;

        }

        class Drink 
        {
            public string Name { get => _name; set { _name = value; } }
            private string _name;
            public List<Ingredient> drinkList { get => _drinkList; set { _drinkList = value; } }
            private List<Ingredient> _drinkList;
            

            public Drink()
            {
                Name = "Coffee";
                drinkList = new List<Ingredient>();
            }

            

           
        }
        class CoffeeModule : Ingredient
        {     
            public CoffeeModule()
            {
                Name = "Coffee";
                Amount = 1000;
            }  
        }

        class WaterModule : Ingredient
        {
            public WaterModule()
            {
                Name = "Water";
                Amount = 1000;
            }
            
        }

        class MilkModule : Ingredient
        {
            public MilkModule()
            {
                Name = "Milk";
                Amount = 1000;
            }
        }

        class SugarModule : Ingredient
        {
            public SugarModule()
            {
                Name = "Sugar";
                Amount = 1000;
            }
        }
    }
}
