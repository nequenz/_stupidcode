using System;
using System.Collections.Generic;

namespace HomeYo
{
    /*
     *      Очень упрощенная реализация Стека, 
     *      Многие методы, исключения, условия и особенности упущены, т.к главная задача показать работу структуры
     */
    public class Stack<typename>
    {
        
        private typename[] Elements;    //Массив пользовательского типа
        private int        Size = 0;    //Использованный размер массива или индекс последнего неиспользованного *адреса массива

        //Конструктор
        public Stack(int Length)
        {
            //Если значение длины массива меньше 0 кидает исключение
            if (Length < 0) throw new Exception("Bad length");

            //Выделяем память под элементы
            Elements = new typename[Length];

        }
        //Наша главная функция, которая кидает элемент в конец списка
        public void push(typename E)
        {
            // Если есть место для нового элемента
            if (Size < Elements.Length) {

        
                Elements[Size] = E; //Кидаем
                Size++;             //Увеличиваем размер
                //Только для дебага функции
                Console.WriteLine(E.ToString() + " was pushed into stack, free places: "+(Elements.Length-Size).ToString());

                return;
            }

            //Иначе пишем, что максимум
            Console.WriteLine("Maximum");



            return;
        }
        //Главная функция получения элемента
        public typename pop()
        {
            //Если значение сайза меньше 0, то возвращаем последний элемент
            if (Size <= 0) 
            {
                Size = 0;               //Код в проекте вышел совсем не качественный, поэтому где нибудь мог не уследить за сайзом, 
                //                          поэтому ставим в 0, т.к теоритическо может быть меньше 0
                return Elements[Size];  // Возвращаем первый элемент в списке\стеке
            }
            /*
             *  Уменьшаем сайз на единицу, т.к при push"е сайз инкриментирует и указываем на размер, а не на последний элемент
             *  p.s, когда сайз уменьшен значение выше его остается в стеке, но при след push, перезапишется новым элементом, а старый 
             *  элемент будет потерян(в с# в этом случае утечки памяти не будет, т.к GC)
             */
            Size--; 
            //Для дебага
            Console.WriteLine("pop value "+Elements[Size]+", free places: "+(Elements.Length - Size).ToString());
            //Возвращаем
            return Elements[Size];
        }

        //Получить последний элемент стека
        public typename peek()
        {

            return Elements[Size - 1];
        }

        //Метод получения кол-ва элементов в стеке 
        public int getSize()
        {
            return Size;
        }

        //Метод получения максимального размера стека
        public int getLength()
        {
            return Elements.Length;
        }

    }



    /*
    *      Очень упрощенная реализация Map, 
    *      Многие методы, исключения, условия и особенности упущены, т.к главная задача показать работу структуры
    *      Также для упрощения были использованы класические списки c#, дабы избежать дополнительного кода для реализации динамического массива да и проблем тоже
    */


    public class Map<TKey, TValue>
    {
        //Списки пользовательского типа
        private List<TKey>   Keys;  //Ключи
        private List<TValue> Values;//Значения

        //Кон-р
        public Map()
        {
            Keys     = new List<TKey>();    
            Values   = new List<TValue>();
        }

        //Метод для добавления пары ключ-значения 

        public void add(TKey K,TValue V)
        {
            Keys.Add(K);
            Values.Add(V);
            //Индекс пары-ключ всегда будет соотвествовать друг другу
        }

        //Метод получения значение по КЛЮЧУ
        public TValue getValue(TKey k)
        {
            //Цикл
            for (int i = 0; i < Values.Count; i++)
            {
                /* Сравнения значений
                 * Т.к класс шаблонный оператор == не подойдет, поэтому для сравнения значений треубется метод quals
                 *
                 */
                if (Keys[i].Equals(k))
                {
                    return Values[i]; //Возвращаем значение
                }
            }

            return default; //возвращем дефолт
        }

        // Получаем размер списка
        public int getLength()
        {
            //Нет разницы какой список, там и там размер одинаковый

            return Keys.Count;
        }

    }




    





    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello bonch!");
            Random Rand = new Random();

            /*
             *      Струтуры данных 
             *      1. Массив одномерный
             *      2. Стек
             *      3. Map
             *  
             */

            //Массив из 10 элементов
            int[] Array = new int[10];

            //Заполняем массива элементами
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = Rand.Next(-500,500);
                Console.WriteLine("Array [" + i.ToString()+"] =" + Array[i]) ;
            }

            // Создаем стек из 10 элементов с типом int
            Stack<int> TestStack = new Stack<int>(10);

            //Закидываем элементы 
            for (int i = 0; i<TestStack.getLength(); i++)
            {
                TestStack.push(Rand.Next(0, 100));
            }


            int Value = TestStack.pop(); //Получаем последний элемент

            //Создаем мап с типом стринг для пары ключ-значение
            Map<String, String> MyMap = new Map<string, string>();

            //Закидываем что нибудь в мап
            MyMap.add("Fruit"  ,"Apple");
            MyMap.add("OS"     ,"Windows");
            MyMap.add("City"   , "SPB");
            MyMap.add("2"      , "Two");
            MyMap.add("Surname", "Name");

            //Получаем и выводим по ключу
            Console.WriteLine( MyMap.getValue("Fruit") );
            Console.WriteLine( MyMap.getValue("OS"   ) );
            Console.WriteLine( MyMap.getValue("City" ) );
        }
    }
}
