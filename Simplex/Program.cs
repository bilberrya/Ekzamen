using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplex
{
    public class Simplexx
    {
        //source - симплекс таблица без базисных переменных
        double[,] table; //симплекс таблица
        public double[] result;
        int x, y;
        List<int> basis; //список базисных переменных
        public Simplexx(double[,] source)
        {
            x = source.GetLength(0);
            y = source.GetLength(1);
            table = new double[x, y + x - 1];
            basis = new List<int>();
            // Добавление фиктивных переменных
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (j < y)
                        table[i, j] = source[i, j];
                    else
                        table[i, j] = 0;
                }
                //выставляем коэффициент 1 перед базисной переменной в строке, это для правильного выстраивания фиктивных переменных, проверка
                if ((y + i) < table.GetLength(1))
                {
                    table[i, y + i] = 1;
                    basis.Add(y + i);
                }
            }
            y = table.GetLength(1);
        }
        //result - в этот массив будут записаны полученные значения X
        public double[,] Calculate(ref double[] result)
        {
            int razrcol, razrrow; //результирующие столбец и строка
            while (!IsItEnd())
            {
                razrcol = findrazrcol();
                razrrow = findrazrrow(razrcol);
                basis[razrrow] = razrcol;
                double[,] new_table = new double[x, y];
                for (int j = 0; j < y; j++)
                    new_table[razrrow, j] = table[razrrow, j] / table[razrrow, razrcol];
                for (int i = 0; i < x; i++)
                {
                    if (i == razrrow)
                        continue;
                    for (int j = 0; j < y; j++)
                        new_table[i, j] = table[i, j] - table[i, razrcol] * new_table[razrrow, j];
                }
                table = new_table;
            }
            //заносим в result найденные значения X
            for (int i = 0; i < result.Length; i++)
            {
                int p = basis.IndexOf(i + 1);
                if (p != -1)
                    result[i] = table[p, 0];
                else
                    result[i] = 0;
            }
            return table;
        }
        private bool IsItEnd() //остановка программы, если строка оценок меньше 0
        {
            bool flag = true;
            for (int j = 1; j < y; j++)
            {
                if (table[x - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
        /// <summary>
        /// метод,, который ищет разрешающий столбец
        /// </summary>
        /// <returns></returns>
        private int findrazrcol()
        {
            int razrcol = 1;
            for (int j = 2; j < y; j++)
                if (table[x - 1, j] < table[x - 1, razrcol])
                    razrcol = j;
            Debug.WriteLine("Разрешающий столбец: "+ razrcol);
            return razrcol;
        }
        /// <summary>
        /// метод,, который ищет разрешающую строку
        /// </summary>
        /// <param name="razrcol"></param>
        /// <returns></returns>
        private int findrazrrow(int razrcol)
        {
            int razrrow = 0;
            for (int i = 0; i < x - 1; i++)
                if (table[i, razrcol] > 0)
                {
                    razrrow = i;
                    break;
                }
            for (int i = razrrow + 1; i < x - 1; i++)
                if ((table[i, razrcol] > 0) && ((table[i, 0] / table[i, razrcol]) < (table[razrrow, 0] / table[razrrow, razrcol])))
                    razrrow = i;
            Debug.WriteLine("Разрешающая строка: " + razrrow);
            return razrrow;
        }
    }
    public class vvoddannyh
    {
        public double[,] mas;
        public double[] bufMass = { };
        public double[,] table_result;
        /// <summary>
        /// Метод ввода и вывода данных
        /// </summary>
        public void simplexBol()
        {
            double[] ms1 = { };
            string str1 = "";
            int raz1 = 0, d = 0;
            //Запись из csv в массив
            try
            {
                using (StreamReader sr = new StreamReader(@"C:\\Users\Анастасия\Desktop\Экзамен\Simplex\Файлы\Ввод.csv"))
                {
                    sr.ReadLine();
                    str1 = sr.ReadToEnd();
                    string[] st = str1.Split('\n');
                    raz1 = st.Length;
                    ms1 = Array.ConvertAll(st[0].Split(';'), double.Parse);
                    d = ms1.Length;
                    mas = new double[raz1, d];
                    for (int i = 0; i < raz1; i++)
                    {
                        ms1 = Array.ConvertAll(st[i].Split(';'), double.Parse);
                        for (int j = 0; j < d; j++)
                        {
                            mas[i, j] = ms1[j];

                        }
                    }

                    // Меняем первый и последний столбцы местами для того что бы удобно вводить в csv файл ограничения
                    for (int i = 0; i < raz1; i++)
                    {
                        for (int j = 0; j < d; j += d - 1)
                        {
                            double tmp = mas[i, j];
                            mas[i, j] = mas[i, d - 1];
                            mas[i, d - 1] = tmp;
                        }

                    }
                    // делаем строку оценок отрицательной для корректного вывода
                    for (int i = 0; i < raz1; i++)
                    {
                        for (int j = 0; j < d; j++)
                        {
                            if (i == raz1 - 1)
                            {
                                mas[i, j] = mas[i, j] * (-1);
                            }
                        }
                    }
                    Console.WriteLine("Исходная матрица");
                    for (int i = 0; i < raz1; i++)
                    {
                        for (int j = 0; j < d; j++)
                        {
                            Console.Write($"{mas[i, j],5}");
                        }
                        Console.WriteLine();
                    }
                }

                //Объявляем массив размерностью в два раза больше, чем введенный массив для фиктивных переменных
                double[] result = new double[raz1 * 2];
                //Конструктор класса
                Simplexx S = new Simplexx(mas);
                //Основной метод программы
                table_result = S.Calculate(ref result);
                for (int i = 0; i < table_result.GetLength(0); i++)
                {
                    for (int j = 0; j < table_result.GetLength(1); j++)
                    {
                        if (i == raz1 - 1)
                        {
                            table_result[i, j] = table_result[i, j] * (-1);
                        }
                    }
                }
                Console.WriteLine("Решение:");
                for (int i = 0; i < table_result.GetLength(0); i++)
                {
                    for (int j = 0; j < table_result.GetLength(1); j++)
                        Console.Write($"{Math.Round(table_result[i, j]),5}" + ";");
                    Console.WriteLine("");
                }
                int ind1 = 1;
                for (int j = d - 2; j >= 0; j--)
                {
                    Console.WriteLine("X[{0}] = {1}", ind1, result[j]);
                    ind1++;
                }
                Console.WriteLine("F = " + (table_result[table_result.GetLength(0) - 1, 0] * -1));
                Console.WriteLine("F' = " + (table_result[table_result.GetLength(0) - 1, 0]));
                using (StreamWriter sw = new StreamWriter(@"Вывод.csv"))
                {
                    sw.WriteLine("reshenie:");
                    for (int i = 0; i < table_result.GetLength(0); i++)
                    {
                        for (int j = 0; j < table_result.GetLength(1); j++)
                            sw.Write($"{Math.Round(table_result[i, j]),5}" + ";");
                        sw.WriteLine();
                    }
                    ind1 = 1;
                    for (int j = d - 2; j >= 0; j--)
                    {
                        sw.WriteLine("X[{0}] = {1}", ind1, result[j]);
                        ind1++;
                    }
                    sw.WriteLine("F = " + (table_result[table_result.GetLength(0) - 1, 0] * -1));
                    sw.WriteLine("F' = " + (table_result[table_result.GetLength(0) - 1, 0]));
                }
            }
            catch
            {
                Console.WriteLine("В файле ошибка, измените данные");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Listeners.Add(new TextWriterTraceListener(File.CreateText("Промежуточные.txt")));
            Debug.AutoFlush = true;
            vvoddannyh vz = new vvoddannyh();
            vz.simplexBol();
            Console.ReadKey();
        }
    }
}
