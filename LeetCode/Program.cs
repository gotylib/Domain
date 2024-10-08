using LeetCode;

ListNode l1 = new ListNode(9);
l1.next = new ListNode(9);
l1.next.next = new ListNode(9);
//l1.next.next.next = new ListNode(4);

ListNode l2 = new ListNode(9);
l2.next = new ListNode(9);
l2.next.next = new ListNode(9);
//l2.next.next.next = new ListNode(4);

ListNode result = Solution.AddTwoNumbers(l1, l2);
while (result != null)
{
    Console.WriteLine(result.val);
    result = result.next;
}

