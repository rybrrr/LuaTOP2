using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NONBINARY_IDENTIFIED_TREE
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }

        class Node<T>
        {
            public T Value { get; set; }
            public int Key { get; set; }

            public Node<T> LeftChild { get; set; }
            public Node<T> RightChild { get; set; }

            public Node(int key, T value)
            {
                Key = key;
                Value = value;
            }
        }

        class BinarySearchTree<T>
        {
            public Node<T> Root { get; set; }

            public string Show()
            {
                void _show(Node<T> node, StringBuilder _sb)
                {
                    if (node == null)
                        return;
                    
                    _show(node.LeftChild, _sb);
                    _show(node.RightChild, _sb);
                }

                StringBuilder sb = new StringBuilder();
                return "";
            }
        }
    }
}
