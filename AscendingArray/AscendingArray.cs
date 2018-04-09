namespace Aa
{
	using System;
	using System.Collections.Generic;
	internal class Node
	{
		public List<Node> candidateChildren = new List<Node>();
		public Node parent;
		public int numAncestors;
		public int val;
	}
	public class Program
	{
		private static List<Node> leaves = new List<Node>();
		private static Node minNode = new Node() {val = Int32.MinValue };
		private static int[] vecSimple = { 1, 2, 3, 4};
		private static int[] vecPlus = {12, 1, 3, 5, 8, 2 };
		private static int[] vecTricky = {12, 1, 3, 5, 8, 1, 2, 4, 9, 10, 11 };
		public static void Main()
		{
			MakeTree(vecTricky);
		}
		private static void MakeTree(int[] vec)
		{
			foreach (int val in vec)
			{
				UpdateTrees(val, leaves);
				TrimTrees(leaves);
			}
			PrintLongestSequence(leaves);
		}
		private static void UpdateTrees(int val, List<Node> leaves)
		{
			Node curNode = minNode;
			foreach (Node node in leaves)
			{
				if (node.val > curNode.val
				  && node.val < val)
				{
					curNode = node;
				}
			}
			if (curNode == minNode)
			{
				leaves.Add( new Node() { parent = null, numAncestors = 0, val = val });
			}
			else
			{
				leaves.Add( new Node { parent = curNode, numAncestors = curNode.numAncestors + 1, val = val});
			}
		}
		private static void TrimTrees(List<Node> leaves)
		{
		}
		private static void PrintLongestSequence(List<Node> leaves)
		{
			Node node = GetLongestSequenceLessThan(leaves);
			if ( node != minNode )
			{
				do
				{
					Console.WriteLine(node.val);
					node = node.parent;
				} while (node != null);
			}
		}
		private static Node GetLongestSequenceLessThan(List<Node> leaves)
		{
			Node curNode = minNode;
			foreach (Node node in leaves)
			{
				if (node.numAncestors > curNode.numAncestors)
				{
					curNode = node;
				}
			}
			return curNode;
		}
	}
}
