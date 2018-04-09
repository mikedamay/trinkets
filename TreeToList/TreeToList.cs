namespace TreeToList
{
	using System;

	public class TreeToList
	{
		public static ListNode _dummy = new ListNode();
		public static void Main()
		{
			(ListNode head, ListNode tail) = ConvertTreeToList(TreeBuilder.Build());
			ListPrinter.PrintList(head);
			ListPrinter.ReversePrintList(tail);
			head.Previous = tail;
			tail.Next = head;
		}
	 	public static (ListNode first, ListNode last) ConvertTreeToList(TreeNode head)
		{
			if (head == null) return (_dummy, _dummy);
			(ListNode firstSmaller, ListNode lastSmaller) = ConvertTreeToList(head.Smaller);
			(ListNode firstLarger, ListNode lastLarger) = ConvertTreeToList(head.Larger);
			ListNode node = new ListNode{ Val = head.Val, Previous = lastSmaller, Next = firstLarger};
			lastSmaller.Next = node;
			firstLarger.Previous = node;
			return (firstSmaller == _dummy ? node : firstSmaller
			  , lastLarger == _dummy ? node : lastLarger);
		}
	}
}
namespace TreeToList
{
	public class TreeNode
	{
		public int Val;
		public TreeNode Smaller;
		public TreeNode Larger;
	}
	public class ListNode
	{
		public int Val;
		public ListNode Previous;
		public ListNode Next;
	}
}
namespace TreeToList
{
	public class TreeBuilder
	{
		public static TreeNode Build3()
		{
			return new TreeNode {Val = 99};
		}
		public static TreeNode Build2()
		{
			return new TreeNode { 
				Val = 4
				,Smaller = new TreeNode {
					Val = 2
					,Smaller = new TreeNode {
						Val = 1
						,Smaller = null, Larger = null }
					,Larger = new TreeNode {
						Val = 3
						,Smaller = null, Larger = null }
					}
				,Larger = new TreeNode {
					Val = 5
					,Smaller = null, Larger = null }
			};
		}
		public static TreeNode Build()
		{
			return new TreeNode { 
				Val = 4
				,Smaller = new TreeNode {
					Val = 2
					,Smaller = new TreeNode {
						Val = 1
						,Smaller = null, Larger = null }
					,Larger = new TreeNode {
						Val = 3
						,Smaller = null, Larger = null }
					}
				,Larger = new TreeNode {
					Val = 8
					,Smaller = new TreeNode {
						Val = 6
						,Smaller = new TreeNode {
							Val = 5, Smaller = null, Larger = null }
						,Larger = new TreeNode {
							Val = 7, Smaller = null, Larger = null }
					}
					, Larger = new TreeNode {
						Val = 9, Smaller = null, Larger = null }
				 }
			};
		}
	}
}
namespace TreeToList
{
	public class ListPrinter
	{
		public static void PrintList(ListNode head)
		{
			ListNode node = head;
			while ( node != TreeToList._dummy )
			{
				System.Console.WriteLine(node.Val);
				node = node.Next;
			}
		}
		public static void ReversePrintList(ListNode tail)
		{
			ListNode node = tail;
			while ( node != TreeToList._dummy )
			{
				System.Console.WriteLine(node.Val);
				node = node.Previous;
			}
		}
	}
}
