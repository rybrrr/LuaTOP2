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
            Node<string> node1 = new Node<string>(1, "Čau");
            Node<string> node2 = new Node<string>(2, "Zdravim");
            Node<string> node3 = new Node<string>(3, "Ahoj");
            Node<string> node4 = new Node<string>(4, "Ahojda");
            Node<string> node5 = new Node<string>(5, "Ahojky");

            node1.LeftChild = node2;
            node1.RightChild = node3;

            node3.LeftChild = node4;
            node3.RightChild = node5;

            BinarySearchTree<string> tree = new BinarySearchTree<string>();
            tree.Root = node1;

            Console.WriteLine(tree.Show());
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
                StringBuilder sb = new StringBuilder();

                void _show(Node<T> node)
                {
                    if (node == null)
                        return;
                    
                    _show(node.LeftChild);
                    sb.Append(node.Key.ToString() + " ");
                    _show(node.RightChild);
                }

                _show(Root);
                return sb.ToString();
            }

            public T? Find(int key)
            {
                // Using recursion for this kind of task is unnecessarily slow

                Node<T> _find(Node<T> node)
                {
                    if (node == null)
                        return null;

                    if (node.Key == key)
                        return node;
                
                    if (node.Key > key)
                        return _find(node.LeftChild);
                    else
                        return _find(node.RightChild);
                }

                Node<T>? leaf = _find(Root);
                T? value = leaf?.Value;

                return value;
            }
        }
    }
}
