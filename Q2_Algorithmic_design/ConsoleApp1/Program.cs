using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static public void Main()
        {
            #region Input Example

            var list1 = new List<Post>() { new Post(15, "post15", "", 15), new Post(45, "", "", 234), new Post(2, "", "", 34), new Post(63, "", "", 3), new Post(12, "", "", 4) };
            var list2 = new List<Post>() { new Post(5, "", "", 4), new Post(23, "", "", 23), new Post(2, "", "", 4), new Post(15, "post155", "", 16) };
            var list3 = new List<Post>() { new Post(67, "", "", 2), new Post(5, "", "", 2), new Post(5, "", "", 1), new Post(63, "", "", 234), new Post(62, "", "", 34) };
            var list4 = new List<Post>() { new Post(52, "", "", 34), new Post(54, "", "", 34), new Post(62, "", "", 23), new Post(86, "", "", 3), new Post(12, "", "", 5) };

            var listofPost = new List<List<Post>>() { list1, list2, list3, list4 };

            #endregion

            var post = merge_posts(listofPost);

            foreach (var item in post)
            {
                Console.WriteLine("createdat: " + item.CreatedAt + " id: " + item.Id + " desc: " + item.Description);
            }

            Console.ReadKey();

        }

        public class Node
        {
            public int data;
            public Node next;
            public Post postData;

            public Node(Post post)
            {
                data = post.CreatedAt;
                next = null;
                postData = post;
            }
        }

        public struct Post
        {
            public Post(int id, string description, string image, int createdAt)
            {
                Id = id;
                Description = description;
                Image = image;
                CreatedAt = createdAt;
            }

            public int Id { get; }
            public string Description { get; }
            public string Image { get; }
            public int CreatedAt { get; }
        } 

        public static List<Post> merge_posts(List<List<Post>> list_of_posts)
        {
            int k = list_of_posts.Count;
            Node[] arr = new Node[k];
            for (int i = 0; i < k; i++)
            {
                list_of_posts[i] = list_of_posts[i].OrderBy(p => p.CreatedAt).ThenBy(n => n.Id).ToList();

                var inode = new Node(list_of_posts[i][0]);
                inode.next = new Node(list_of_posts[i][1]);
                var tempNode = inode.next;

                for (int j = 2; j < list_of_posts[i].Count; j++)
                {
                    tempNode.next = new Node(list_of_posts[i][j]);
                    tempNode = tempNode.next;
                }

                arr[i] = inode;
            }

            var mergedListNode = mergeLists(arr, k - 1);

            var posts = ConvertNodeToPostList(mergedListNode).GroupBy(p => p.Id).Select(p => p.First()).OrderBy(p => p.Id).ToList();

            return posts.OrderByDescending(p => p.CreatedAt).ThenByDescending(n => n.Id).ToList();
        }

        static Node mergeLists(Node[] arr, int last)
        {
            for (int i = 1; i <= last; i++)
            {
                while (true)
                {
                    Node head_0 = arr[0];
                    Node head_i = arr[i];

                    if (head_i == null)
                        break;

                    if (head_0.data >= head_i.data)
                    {
                        arr[i] = head_i.next;
                        head_i.next = head_0;
                        arr[0] = head_i;
                    }
                    else
                    {
                        while (head_0.next != null)
                        {
                            if (head_0.next.data >= head_i.data)
                            {
                                arr[i] = head_i.next;
                                head_i.next = head_0.next;
                                head_0.next = head_i;
                                break;
                            }

                            head_0 = head_0.next;

                            if (head_0.next == null)
                            {
                                arr[i] = head_i.next;
                                head_i.next = null;
                                head_0.next = head_i;
                                head_0.next.next = null;
                                break;
                            }
                        }
                    }
                }
            }
            return arr[0];
        }

        static List<Post> ConvertNodeToPostList(Node node)
        {
            var posts = new List<Post>();
            while (node != null)
            {
                posts.Add(node.postData);
                node = node.next;
            }
            return posts;
        }
    }
}