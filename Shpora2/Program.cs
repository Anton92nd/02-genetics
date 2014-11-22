using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpora2
{
	class Program
	{
		public static int[] ar;

		public static List<bool> ListFromMask(int mask,int  n)
		{
			var res = new List<bool>();
			for (int i = 0; i < n; i++)
			{
				res.Add((i & 1) == 1);
			}
			return res;
		}

		static void Main(string[] args)
		{
			var inp = File.ReadAllText("../../input.txt");
			var lstar = inp.Split(' ').Select(int.Parse).ToList();
			int n = lstar[0];
			lstar.RemoveAt(0);
			ar = lstar.ToArray();
			var masks = new List<bool>[ar.Length];
			var rand = new Random();
			for (int i = 0; i < masks.Length; i++)
			{
				masks[i] = new List<bool>();
				for (int j = 0; j < n; j++)
				{
					masks[i].Add(rand.Next(1) == 0);
				}
			}
			List<bool>[] newGen = masks;
			for (int it = 0; it < 100000; it++)
			{
				newGen = Inherit(newGen, n);
			}
			var ans = newGen.First();
			Console.WriteLine(Fitness(ans));
			for (int i = 0; i < n; i++)
			{
				if (ans[i])
				{
					Console.Write(ar[i] + " ");
				}
			}
			Console.WriteLine();
			for (int i = 0; i < n; i++)
			{
				if (!ans[i])
				{
					Console.Write(ar[i] + " ");
				}
			}
		}

		private static void GetMask(int mask, int n)
		{
			for (int i = 0; i < n; i++)
			{
				if ((mask & (1 << i)) > 0)
				{
					Console.Write(ar[i] + " ");
				}
			}
			Console.WriteLine();
			for (int i = 0; i < n; i++)
			{
				if ((mask & (1 << i)) == 0)
				{
					Console.Write(ar[i] + " ");
				}
			}
		}

		static List<bool> GetXor(List<bool> a, List<bool> b)
		{
			var res = new List<bool>();
			for (var i = 0; i < a.Count; i++)
			{
				res.Add(a[i] ^ b[i]);
			}
			return res;
		}

		static List<bool>[] Inherit(List<bool>[] mask, int n)
		{
			var r = new Random();
			var res = new List<List<bool>>(mask);
			for (int i = 0; i < 10; i++)
			{
				int a = r.Next(mask.Length);
				int b = r.Next(mask.Length);
				if (a != b)
				{
					res.Add(GetXor(mask[a], mask[b]));
				}
			}
			for (int i = 0; i < 10; i++)
			{
				int a = r.Next(res.Count);
				int bit = r.Next(n);
				res[a][bit] ^= true;
			}
			return res.OrderBy(Fitness).Take(15).ToArray();
		}

		public static int Fitness(List<bool> lst)
		{
			int a = 0, b = 0;
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i])
				{
					a += ar[i];
				}
				else
				{
					b += ar[i];
				}
			}
			return Math.Abs(a - b);
		}
	}
}
