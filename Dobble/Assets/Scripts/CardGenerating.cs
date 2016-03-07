using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// http://radiganengineering.com/2013/01/spot-it-howd-they-do-that/

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // constraint; prime
            int p = 2;

            int picturesPerCard = p + 1, numberOfCards = p * p + p + 1;
            int[,] cards = new int[numberOfCards, picturesPerCard];
            int minFactor = 2, boundMinFactor = Convert.ToInt32(Math.Sqrt(p));
            while (minFactor <= boundMinFactor && p % minFactor != 0) ++minFactor;
            if (minFactor > boundMinFactor) minFactor = p;
            int row = 0;
            for (int i = 0; i < p; ++i) {
                for (int j = 0; j < p; ++j) cards[row, j] = i * p + j;
                cards[row, p] = p * p;
                ++row;
            }
            for (int i = 0; i < minFactor; ++i) {
                for (int j = 0; j < p; ++j) {
                    for (int k = 0; k < p; ++k) cards[row, k] = k * p + (j + i * k) % p;
                    cards[row, p] = p * p + 1 + i;
                    ++row;
                }
            }
            for (int i = 0; i <= minFactor; ++i) cards[row, i] = p * p + i;

            // optional; writing to console
            for (row = 0; row < numberOfCards; ++row) {
                for (int i = 0; i <= p; ++i) Console.Write(cards[row, i] + " ");
                Console.WriteLine();
            }

            // optional; wait before exit
            Console.ReadLine();
        }
    }
}
 