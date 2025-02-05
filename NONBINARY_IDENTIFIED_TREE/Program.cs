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

            public Node<T>? Parent { get; set; }

            public Node(int key, T value)
            {
                Key = key;
                Value = value;
            }

            public void Unparent()
            {
                if (node.Parent == null)
                    return;

                if (node.Parent.LeftChild == node)
                    node.Parent.LeftChild = null;
                else if (node.Parent.RightChild == node)
                    node.Parent.RightChild = null;
            }

            public void ClearChildren()
            {
                LeftChild = null;
                RightChild = null;
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

            public Node<T>? Find(int key)
            {
                Node<T>? node = Root;
                while (node != null)
                {
                    if (node.Key == key)
                        return node;

                    if (node.Key < key)
                        node = node.Right;
                    else if (node.Key > key)
                        node = node.Left
                }
            }

            public Node<T>? FindMin(Node<T>? start)
            {
                Node<T>? node = start == null ? Root : start;
                while (node != null)
                {
                    node = node.LeftChild;
                }
                return node;
            }

            public Node<T>? FindMax(Node<T>? start)
            {
                Node<T>? node = start == null ? Root : start;
                while (node != null)
                {
                    node = node.RightChild;
                }
                return node;
            }

            public void InsertNode(Node<T> newNode, Node<T>? start)
            {
                if (Root == null)
                {
                    Root = newNode;
                    return;
                }

                Node<T>? lastNode = null;
                Node<T>? node = start == null ? Root : start;
                while (node != null)
                {
                    lastNode = node;
                    if (node.Key == newNode.Key)
                        return;    // Key already exists in the tree => no need to change anything

                    if (node.Key < newNode.Key)
                        node = node.Right;
                    else if (node.Key > newNode.Key)
                        node = node.Left;
                }

                newNode.Parent = lastNode;
                if (lastNode.Key < newNode.Key)
                    lastNode.Right = newNode;
                else
                    lastNode.Left = newNode;
            }

            public void Insert(int newKey, T newValue)
            {
                Node<T> newNode = new Node<T>(newKey, newValue);
                InsertNode(newNode);
            }

            public void RemoveNode(Node<T> node)
            {
		Node<T>? substituent = null;
                substituent = substituent == null ? FindMax(node.LeftChild) : substituent;
                substituent = substituent == null ? FindMin(node.RightChild) : substituent;

                Node<T>? parent = node.Parent;
                Node<T>? rightChild = node.RightChild;

                node.Unparent();
                node.ClearChildren();

                if (substituent == null)
                    return;
                
                substituent.Unparent();
                InsertNode(substituent, parent);

                if (parent == null && rightChild != null)
                    InsertNode(rightChild, substituent);

            }

            public T? Remove(int key)
            {
                Node<T>? node = Find(key);
                if (node == null)
                    return null;

                
            }
        }
    }
}
