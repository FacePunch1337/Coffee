using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;

namespace Coffee
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Ingredient>));
            List<Ingredient> data = null;
            DrinkMaker drinkMaker = new DrinkMaker();
            try
            {
                using (FileStream fs = new FileStream("Save.xml", FileMode.OpenOrCreate))    //| Десериализация
                {                                                                            //|      состояний
                    data = xmlSerializer.Deserialize(fs) as List<Ingredient>;                //|        модулей
                }
            }
            catch (Exception ex) { }

            if (data != null)
            {
                data = drinkMaker.MakeDrink(data);
            }
            else data = drinkMaker.MakeDrink();


            using (FileStream fs = new FileStream("Save.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))      //|   Сериализация
            {                                                                                                                   //|      состояний
                xmlSerializer.Serialize(fs, data);                                                                              //|        модулей         
            }

        }

    }
    class DrinkMaker : Menu
    {

        public Drink Drink { get => _drink; set { _drink = value; } }
        private Drink _drink;       //ссылка на объект напитка

        public double Qantity_Of_Ingredients { get => _quantity_of_ingredients; set { _quantity_of_ingredients = value; } }
        private double _quantity_of_ingredients;        //поле запомниающее передаваемое значение о колличестве ингредиента

        public bool check { get => _check; set { _check = value; } }
        private bool _check;        //поле запоминающее результат проверки на наличие ингредиентов в модулях



        public DrinkMaker()
        {
            Drink = new Drink();

        }


        public List<Ingredient> MakeDrink(List<Ingredient> data)
        {
            Coffee = (CoffeeModule)data[0];
            Water = (WaterModule)data[1];
            Milk = (MilkModule)data[2];
            Sugar = (SugarModule)data[3];
            return MakeDrink();
        } // метод присваивающий полям ингридиентов их данные (Для сериализации)
        public List<Ingredient> MakeDrink()
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
                Console.WriteLine("4] Сappuccino");
                Console.WriteLine("\nTab] Storage");
                Console.WriteLine("+] AdminPanel");
                Console.WriteLine("Esc] Exit");
                Console.WriteLine("______________");
                Console.WriteLine('\t');
                choice = Console.ReadKey(true).KeyChar;
                switch (choice)
                {

                    case '1':
                        if (CreateEspresso() == true) { DoYouWannaSugar(); Loading(); ShowDrink(); } //если метод создания конкретного напитка вернет true - то он будет сделан
                        else Console.WriteLine("Missing ingredients");
                        break;
                    case '2':
                        if (CreateAmericano() == true) { DoYouWannaSugar(); Loading(); ShowDrink(); }
                        else Console.WriteLine("Missing ingredients");
                        break;
                    case '3':
                        if (CreateLatte() == true) { DoYouWannaSugar(); Loading(); ShowDrink(); }
                        else Console.WriteLine("Missing ingredients");
                        break;
                    case '4':
                        if (CreateСappuccino() == true) { DoYouWannaSugar(); Loading(); ShowDrink(); }
                        else Console.WriteLine("Missing ingredients");
                        break;
                    case (char)ConsoleKey.Tab:
                        Storage();
                        break;
                    case '+':
                        AdminPanel();
                        break;
                }

            } while (choice != (char)ConsoleKey.Escape);

            return new List<Ingredient> { Coffee, Water, Milk, Sugar };
        } // метод создания напитка
        public void AddToDrink(Ingredient item, double amount)
        {


            switch (item.Name)
            {

                case "Coffee":
                    Drink.coffee_qantity = amount; //кол-во ингридиентов 
                    if (AmountChecker(item) == true) //если метод проверки наличия ингредиентов вернёт true - то ингредиенты будут добавлены 
                    {

                        Drink.drinkList.Add(item); //добавляю ингридиент в напиток
                        item.Amount = Coffee.Amount - Drink.coffee_qantity; //отнимаю входящее кол-во ингридиентов от общего - получаю остаток  

                    }


                    break;
                case "Water":
                    Drink.water_qantity = amount;
                    if (AmountChecker(item) == true)
                    {
                        Drink.drinkList.Add(item);
                        item.Amount = Water.Amount - Drink.water_qantity;
                    }

                    break;
                case "Milk":
                    Drink.milk_qantity = amount;
                    if (AmountChecker(item) == true)
                    {
                        Drink.drinkList.Add(item);
                        item.Amount = Milk.Amount - Drink.milk_qantity;
                    }

                    break;
                case "Sugar":
                    Drink.sugar_qantity = amount;
                    if (AmountChecker(item) == true)
                    {
                        Drink.drinkList.Add(item);
                        item.Amount = Sugar.Amount - Drink.sugar_qantity;
                        
                    }

                    break;
            }

        } // метод добавление ингредиента в напиток (ингредиент, колличество)

        public bool CreateEspresso()
        {
            AddToDrink(Coffee, 7);
            AddToDrink(Water, 50);

            return check;
        } // сделать еспрессо 
        public bool CreateAmericano()
        {
            AddToDrink(Coffee, 7);
            AddToDrink(Water, 150);

            return check;
        } // сделать американо 
        public bool CreateLatte()
        {
            AddToDrink(Coffee, 7);
            AddToDrink(Water, 50);
            AddToDrink(Milk, 150);


            return check;
        } // сделать латте 
        public bool CreateСappuccino()
        {
            AddToDrink(Coffee, 14);
            AddToDrink(Water, 50);
            AddToDrink(Milk, 100);


            return check;
        } // сделать капучино 

        public bool AmountChecker(Ingredient items) //Метод проверяющий колличество ингридиентов 
        {
            //метод вернёт true только в том случае если кол-во всех ингредиентов в запасе будет минимальное

            if (Coffee.Amount < 14 || // 14 - минимальное количество кофе в запасе
               Water.Amount < 150 || //150 - минимальное количество воды в запасе
               Milk.Amount < 150 || // 100 - минимальное количество молока в запасе
               Sugar.Amount < 1) //1 - минимальное количество сахара в запасе
            {
                return check = false;
            }
            else
            {
                return check = true;
            }



        }
        public void DoYouWannaSugar()
        {
            Console.Clear();
            Console.WriteLine("Sugar?\n1] Yes.\n2] No.\n\n");
            var choice = Console.ReadKey(true).KeyChar;
            switch (choice)
            {
                case '1':

                    Console.WriteLine("Spoon: ");
                    var spoon = (char)Console.ReadKey(true).KeyChar - '0';

                    AddToDrink(Sugar, spoon);  // если да - то добавляет указанное кол-во ложек   

                    break;
                case '2':

                    break;
            }
            Console.Clear();
        } // метод добавления сахара (отдельно, так как сахар на выбор)
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
            Console.WriteLine("READY");
        } //метод имитирующий процесс загрузки(приготовления)
        public void ShowDrink()
        {
            int i = 1;
            foreach (var item in Drink.drinkList)
            {
                if (item.Name == "Coffee")
                    Console.WriteLine($"{i++}] {item.Name}\t{Drink.coffee_qantity} gr");
                if (item.Name == "Water")
                    Console.WriteLine($"{i++}] {item.Name}\t{Drink.water_qantity} ml");
                if (item.Name == "Milk")
                    Console.WriteLine($"{i++}] {item.Name}\t\t{Drink.milk_qantity} ml");
                if (item.Name == "Sugar")
                    Console.WriteLine($"{i++}] {item.Name}\t{Drink.sugar_qantity} gr");
            }
            Drink.coffee_qantity = 0;
            Drink.water_qantity = 0;
            Drink.milk_qantity = 0;
            Drink.sugar_qantity = 0;
        } // метод показывающий напиток как список ингредиентов
        public void Storage()
        {

            int id = 1;
            Console.WriteLine("\n");
            Console.WriteLine($"{id++}]{Coffee.Name} = {Coffee.Amount} gr");
            Console.WriteLine($"{id++}]{Water.Name} = {Water.Amount} ml");
            Console.WriteLine($"{id++}]{Milk.Name} = {Milk.Amount} ml");
            Console.WriteLine($"{id++}]{Sugar.Name} = {Sugar.Amount} gr");
        } // метод показывающй общее кол-во ингредиентов в запасе
        public bool AdminPanel()
        {

            // доступ выдаётся при вводе пароля
            bool access;

            Console.Clear();
            Console.WriteLine("Enter Password:");
            string password = Console.ReadLine();
            if (password == "1986") //пока что хард код
            {
                access = true;
                Console.Clear();
                Console.WriteLine("access is allowed.");

                Console.WriteLine("1] Edit storage");
                Console.WriteLine("\nBackSpace] Leave admin panel");

                var choice = Console.ReadKey(true).KeyChar;

                switch (choice)
                {
                    case '1':
                        EditStorage(); // метод изменения данных в хранилище (добавить/убавить)
                        break;

                }

                void EditStorage()
                {
                    char choice;


                    do
                    {

                        Console.Clear();
                        Console.WriteLine("\nBackSpace] Leave admin panel");
                        Storage();

                        Console.WriteLine("\nChoice ID");
                        choice = Console.ReadKey(true).KeyChar;

                        switch (choice)
                        {
                            case '1':
                                EditStorage(Coffee);
                                break;
                            case '2':
                                EditStorage(Water);
                                break;
                            case '3':
                                EditStorage(Milk);
                                break;
                            case '4':
                                EditStorage(Sugar);
                                break;



                        }


                        void EditStorage(Ingredient item)
                        {
                            double amount_ingredient = item.Amount;


                            char refuel;
                            amount_ingredient = item.Amount;
                            Console.WriteLine("+] Add");
                            Console.WriteLine("-] Remove");

                            refuel = Console.ReadKey(true).KeyChar;
                            if (refuel == '+')
                            {
                                amount_ingredient += 100;
                                AddToStorage(item, amount_ingredient);
                                Console.Clear();
                                Storage();
                            }
                            if (refuel == '-')
                            {
                                amount_ingredient -= 100;
                                AddToStorage(item, amount_ingredient);
                                Console.Clear();
                                Storage();
                            }
                            void AddToStorage(Ingredient item, double amount)
                            {
                                item.Amount = amount;
                            }
                        } //вложеный перегруженый метод принимающий ингредиент в качестве парраметра
                    } while (choice != (char)ConsoleKey.Backspace);

                } // метод изменения данных в хранилище
            }
            else
            {
                access = false;
                Console.Clear();
                Console.WriteLine("access is denied!");

            }
            return access;

        } // метод вызывающий панель администратора для управлние модулями (в разработке)

    } // модуль управления

    class Drink
    {
        public string Name { get => _name; set { _name = value; } }
        private string _name;
        public List<Ingredient> drinkList { get => _drinkList; set { _drinkList = value; } }
        private List<Ingredient> _drinkList;    //список ингредиентов в напитке


        //поля запоминающие кол-во различных ингредиентов в напитке
        public double coffee_qantity { get => _coffee_qantity; set { _coffee_qantity = value; } }
        private double _coffee_qantity;

        public double water_qantity { get => _water_qantity; set { _water_qantity = value; } }
        private double _water_qantity;

        public double milk_qantity { get => _milk_qantity; set { _milk_qantity = value; } }
        private double _milk_qantity;

        public double sugar_qantity { get => _sugar_qantity; set { _sugar_qantity = value; } }
        private double _sugar_qantity;



        public Drink()
        {
            drinkList = new List<Ingredient>();
            coffee_qantity = 0;
            water_qantity = 0;
            milk_qantity = 0;
            sugar_qantity = 0;
        }
        public Drink(Drink drink)
        {

            Name = "Coffee";

        }




    } // класс хранящий представление о напитке
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

        public Menu()
        {
            Coffee = new CoffeeModule();
            Water = new WaterModule();
            Milk = new MilkModule();
            Sugar = new SugarModule();
        }



    } // класс меню



    [Serializable]                       //| Сериализация 
    [XmlInclude(typeof(CoffeeModule))]   //|    состояний
    [XmlInclude(typeof(WaterModule))]    //|      модулей
    [XmlInclude(typeof(MilkModule))]     //|
    [XmlInclude(typeof(SugarModule))]    //|
    public class Ingredient
    {

        public string Name { get => _name; set { _name = value; } }
        private string _name;
        public double Amount { get => _amount; set { _amount = value; } }
        public double _amount;




    } //Класс хранящий представление об ингредиенте

    [Serializable]
    public class CoffeeModule : Ingredient
    {
        public CoffeeModule()
        {
            Name = "Coffee";
            Amount = 1000;
        }

    } // модуль кофе

    [Serializable]
    public class WaterModule : Ingredient
    {

        public WaterModule()
        {
            Name = "Water";
        }

    } // модуль воды

    [Serializable]
    public class MilkModule : Ingredient
    {
        public MilkModule()
        {

            Name = "Milk";
            Amount = 1000;

        }



    } // модуль молока

    [Serializable]
    public class SugarModule : Ingredient
    {

        public SugarModule()
        {
            Name = "Sugar";
            Amount = 1000;
        }
    }// модуль сахара
}

