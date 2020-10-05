using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

// 전체적으로 연산부분이 곂치는 부분이 많음 이를 나누고 묶는 독립성과 응집도에 대한 다듬음이 필요한코드
// 아마 굳이 Exception 을상속하지 않는다면 PMath에 반복적인 연산을 묶고 이를 가지고 one, two, three 를 상속받거나
// one -> two -> three 과정으로 상속받는 편이 나았을 코드

namespace test01
{
    enum Arithmetic { Add = 1, Sub = 2, Mul = 3, Div = 4 }
    enum Choose { One = 1, Two, Three }
    class PMath
    {
        int x = 0, y = 0;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public int Add(out char ari)
        {
            ari = '+';
            return x + y;
        }
        public int Sub(out char ari)
        {
            ari = '-';
            return x - y;
        }
        public int Mul(out char ari)
        {
            ari = '*';
            return x * y;
        }
        public int Div(out char ari)
        {
            ari = '/';
            return x / y;
        }

        public int Make(Choose choose, out char ari)     //자릿수를 결정하기 위한 choose형 및 연산자를 확인하기위한 ari
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            int temp = 0;
            switch (choose)
            {
                case Choose.One:
                    X = random.Next(1, 10);
                    Y = random.Next(1, 10);
                    break;
                case Choose.Two:
                    X = random.Next(10, 100);
                    Y = random.Next(10, 100);
                    break;
                case Choose.Three:
                    X = random.Next(100, 1000);
                    Y = random.Next(100, 1000);
                    break;
            }
            ari = ' ';
            int arithmetic = random.Next(1, 5); //1~5 연산자
            switch (arithmetic)
            {
                case (int)Arithmetic.Add: temp = this.Add(out ari); ari = '+'; break;
                case (int)Arithmetic.Sub: temp = this.Sub(out ari); ari = '-'; break;
                case (int)Arithmetic.Mul: temp = this.Mul(out ari); ari = '*'; break;
                case (int)Arithmetic.Div: temp = this.Div(out ari); ari = '/'; break;
            }
            return temp;
        }
    }


    class OneException : Exception
    {
        PMath pMath = new PMath();

        public virtual int RUN(Choose choose = Choose.One)
        {
            while (true)
            {
                try
                {
                    char ari;
                    int temp = pMath.Make(choose, out ari), input = 0;
                    Console.WriteLine("다음의 답을 입력하세요.");
                    Console.Write($"{pMath.X} {ari} {pMath.Y} = ");
                    input = int.Parse(Console.ReadLine());
                    if (input != temp)
                    {
                        Console.WriteLine("틀렸습니다.");
                        Console.ReadLine();
                        Console.Clear();
                        return 0;
                    }
                    else
                    {
                        Console.WriteLine("정답입니다.");
                        Console.ReadLine();
                        Console.Clear();
                        return 1;
                    }
                }
                catch
                {
                    Console.WriteLine("잘못 입력 되었습니다");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }
    }

    class TwoException : OneException
    {
        PMath pMath = new PMath();


        public override int RUN(Choose choose = Choose.Two)
        {
            int win = 0, lose = 0;

            while (true)
            {
                int temp =  base.RUN(choose);   //one
                if (temp == 0)
                {
                    lose++;
                    Console.Clear();
                }
                else
                {
                    win++;
                    Console.Clear();
                }

                if (win + lose >= 5)
                {
                    break;
                }
            }

            Console.WriteLine($"정답수: {win}, 오답수: {lose}, 승률: {((win / (double)(win + lose)) * 100).ToString("#") + "%"}");
            return (int)((win / (double)(win + lose)) * 100);
        }
    }

    class ThreeException : TwoException
    {
        PMath pMath = new PMath();

        public void RUN()
        {
            Choose choose = Choose.One;
            int temp = 80;
            try
            {
                while (true)
                {
                    if (temp >= 80)
                    {

                        Console.WriteLine("난이도를 선택하세요");
                        Console.WriteLine("1. 쉬움, 2. 중간, 3. 어려움");
                        int input = int.Parse(Console.ReadLine());
                        switch (input)
                        {
                            case (int)Choose.One:
                                choose = Choose.One;
                                break;
                            case (int)Choose.Two:
                                choose = Choose.Two;
                                break;
                            case (int)Choose.Three:
                                choose = Choose.Three;
                                break;
                            default:
                                continue;
                        }
                    }
                    temp = base.RUN(choose);    //Tow
                }
            }
            catch (Exception)
            {
                Console.WriteLine("잘못 입력 되었습니다");
                Console.ReadLine();
                Console.Clear();
            }

        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("하실 프로그램을 선택하세요\n1. 유형1\t2. 유형2\t3. 유형3");
                try
                {
                    int input = int.Parse(Console.ReadLine());
                    switch (input)
                    {
                        case (int)Choose.One: throw new OneException();
                        case (int)Choose.Two: throw new TwoException();
                        case (int)Choose.Three: throw new ThreeException();

                        default: throw new Exception("입력이 잘못되었습니다.");
                    }
                }
                catch (ThreeException err)
                {
                    err.RUN();
                    break;
                }
                catch (TwoException err)
                {
                    err.RUN();
                    break;
                }
                catch (OneException err)
                {
                    while (true)
                    {
                        int temp = err.RUN();
                        if(temp == 1)
                        {
                            Console.WriteLine("계속할려면 1 아니면 다른 키 입력");
                            ConsoleKeyInfo console  = Console.ReadKey();
                            if(console.Key == ConsoleKey.D1 || console.Key == ConsoleKey.NumPad1)
                            {
                                Console.ReadLine();
                                continue;

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                }
                catch (Exception err)        //Exception의 순서 중요
                {
                    Console.WriteLine(err.Message);
                }
            }
        }
    }
}