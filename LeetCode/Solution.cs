using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode
{
    public class ListNode {
        public int val;
        public ListNode next;
        public ListNode(int val=0, ListNode next=null) {
            this.val = val;
            this.next = next;
        }
    }
 
    public static class Solution
    {
        public  static ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            int remains = 0;
            ListNode result = new ListNode();
            ListNode iterator = result;
            while (l1 != null || l2 != null)
             {
                if (l1 == null)
                {
                    remains = l2.val + remains;
                    if(remains > 9)
                    {
                        iterator.val = remains == 10 ? 0 : (remains - 10);
                        remains = 1;
                    }
                    else
                    {
                        iterator.val = remains;
                        remains = 0;
                    }
                }
                else if (l2 == null)
                {
                    remains = l1.val + remains;
                    if (remains > 9)
                    {
                        iterator.val = remains == 10 ? 0 : (remains - 10);
                        remains = 1;
                    }
                    else
                    {
                        iterator.val = remains;
                        remains = 0;
                    }
                }
                else
                {
                    remains = l1.val + l2.val + remains;
                    if (remains > 9)
                    {
                        iterator.val = remains == 10 ? 0 : (remains - 10);
                        remains = 1;
                    }
                    else
                    {
                        iterator.val = remains;
                        remains = 0;
                    }
                }
                l1 = l1.next;
                l2 = l2.next;  
                if(l1 != null || l2 != null)
                {
                    iterator.next = new ListNode();
                    iterator = iterator.next;
                }
             }
            if (remains != 0)
            {
                iterator.next = new ListNode(remains);
            }
            return result;
        }
    }
}
