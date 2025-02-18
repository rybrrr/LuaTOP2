using System.Text;

namespace NONBINARY_IDENTIFIED_TREE
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BinarySearchTree<Student> tree = new BinarySearchTree<Student>();

            using(StreamReader streamReader = new StreamReader("studenti_shuffled.csv"))
            {   
                string? line = streamReader.ReadLine();
                while (line != null)
                {
                    string[] studentData = line.Split(',');

                    Student student = new Student(
                        Convert.ToInt32(studentData[0]),    // Id
                        studentData[1],                     // Jméno
                        studentData[2],                     // Příjmení
                        Convert.ToInt16(studentData[3]),    // Věk
                        studentData[4]                      // Třída
                    );

                    tree.Insert(student.Id, student);
                    line = streamReader.ReadLine();
                }
            }

            // Najděte studenta s ID 20 (David Urban (ID: 20) ze třídy 4.A)
            Console.Write("1.");
            Console.WriteLine(tree.Find(20)?.Value);

            // Najděte studenta s nejnižším ID (Kateřina Sedláček (ID: 1) ze třídy 1.B)
            Console.Write("2. ");
            Console.WriteLine(tree.FindMin(null)?.Value);

            // Vložte vlastního studenta s ID > 100 (je potřeba vytvořit nový objekt typu Student) a zkuste ho pak najít
            Student newStudent = new Student(
                523971,
                "Vít",
                "Jahůdka",
                38,
                "3.C"
            );
            tree.Insert(newStudent.Id, newStudent);
            Console.Write("3.1. ");
            Console.WriteLine(tree.Find(523971)?.Value);
            Console.Write("3.2. ");
            Console.WriteLine(tree.FindMax(null)?.Value);
            
            // Smažte všechny studenty se sudým ID
            for (int i=2; i<=100; i+=2)
            {
                tree.Remove(i);
            }

            // Vypište strom (měli byste vidět jen ID lichá a seřazená)
            Console.Write("4. ");
            Console.WriteLine(tree.Show());
        }
    }

    class Node<T>
    {
        public T Value { get; set; }
        public int Key { get; set; }

        public Node<T>? LeftChild { get; set; }
        public Node<T>? RightChild { get; set; }

        public Node<T>? Parent { get; set; }

        public Node(int key, T value)
        {
            Key = key;
            Value = value;
        }

        public void Unparent()
        {
            if (Parent == null)
                return;

            if (Parent.LeftChild == this)
                Parent.LeftChild = null;
            else if (Parent.RightChild == this)
                Parent.RightChild = null;
            
            Parent = null;
        }

        public void ClearChildren()
        {
            LeftChild = null;
            RightChild = null;
        }

        public void InsertChild(Node<T> child)
        {
            child.Parent = this;
            if (Key < child.Key)
                RightChild = child;
            else if (Key > child.Key)
                LeftChild = child;
        }
    }

    class BinarySearchTree<T>
    {
        public Node<T>? Root { get; set; }

        #pragma warning disable 8604
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
        #pragma warning restore 8604

        public Node<T>? Find(int key)
        {
            Node<T>? node = Root;
            while (node != null)
            {
                if (node.Key == key)
                    return node;

                if (node.Key < key)
                    node = node.RightChild;
                else if (node.Key > key)
                    node = node.LeftChild;
            }
            return null;
        }

        public Node<T>? FindMin(Node<T>? start)
        {
            Node<T>? node = start == null ? Root : start;
            if (node == null)
                return null;

            while (true)
            {
                if (node.LeftChild == null)
                    return node;

                node = node.LeftChild;
            }
        }

        public Node<T>? FindMax(Node<T>? start)
        {
            Node<T>? node = start == null ? Root : start;
            if (node == null)
                return null;

            while (true)
            {
                if (node.RightChild == null)
                    return node;

                node = node.RightChild;
            }
        }

        public void UnparentNode(Node<T> node)
        {
            node.Unparent();
            if (Root == node)
                Root = null;
        }

        public void InsertNode(Node<T> newNode, Node<T>? start)
        {
            if (Root == null)
            {
                Root = newNode;
                return;
            }

            Node<T>? node = start == null ? Root : start;
            Node<T> lastNode = node;
            while (node != null)
            {
                lastNode = node;
                if (node.Key == newNode.Key)
                    return;    // Key already exists in the tree => no need to change anything

                if (node.Key < newNode.Key)
                    node = node.RightChild;
                else if (node.Key > newNode.Key)
                    node = node.LeftChild;
            }

            lastNode.InsertChild(newNode);
        }

        public void Insert(int newKey, T newValue)
        {
            Node<T> newNode = new Node<T>(newKey, newValue);
            InsertNode(newNode, null);
        }

        public void ReplaceNode(Node<T> replacee, Node<T> replacant)
        {
            Node<T>? parent = replacee.Parent;
            Node<T>? leftChild = replacee.LeftChild;
            Node<T>? rightChild = replacee.RightChild;

            if ((replacant != leftChild && replacant != rightChild)
            || (replacant == leftChild && rightChild != null && replacant.RightChild != null) 
            || (replacant == rightChild && leftChild != null && replacant.LeftChild != null))
                RemoveNode(replacant);  // Remove node only if it can't just move the replacant one layer up without consequences

            UnparentNode(replacee);
            replacee.ClearChildren();

            InsertNode(replacant, parent);  // Inserts as the root if needed
            if (leftChild != null && leftChild != replacant)
                replacant.InsertChild(leftChild);
            if (rightChild != null && rightChild != replacant)
                replacant.InsertChild(rightChild);
        }

        public void RemoveNode(Node<T> node)
        {
            Node<T>? substituent = null;
            substituent = substituent == null && node.LeftChild != null ? FindMax(node.LeftChild) : substituent;
            substituent = substituent == null && node.RightChild != null ? FindMin(node.RightChild) : substituent;

            if (substituent == null)    // it's a leaf
            {
                UnparentNode(node);
                return;
            }

            ReplaceNode(node, substituent);
        }

        public void Remove(int key)
        {
            Node<T>? node = Find(key);
            if (node == null)
                return;

            RemoveNode(node);
        }
    }

    class Student
    {
        public int Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int Age { get; }

        public string ClassName { get; }

        public Student(int id, string firstName, string lastName, int age, string className)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            ClassName = className;
        }
        
        public override string ToString()
        {
            return string.Format("{0} {1} (ID: {2}) ze třídy {3}",FirstName,LastName,Id,ClassName);
        }
    }
}
