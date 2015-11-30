using System.Collections.Generic;

namespace BookStore.Models {
    public class Cart {

        private IDictionary<int, int> content;

        public IDictionary<int, int> Content {
            get {
                return content;
            }
        }

        public Cart() {
            content = new Dictionary<int, int>();
        }

        public void AddBook(int id) {
            if (Content.Keys.Contains(id)) {
                Content[id]++;
            } else {
                Content.Add(id, 1);
            }
        }

        public void RemoveBook(int id) {
            if (Content.Keys.Contains(id)) {
                if (Content[id] > 1) {
                    Content[id]--;
                } else {
                    Content.Remove(id);
                }
            }
        }

        public bool IsEmpty() {
            return content.Count == 0;
        }

        public void Clear() {
            Content.Clear();
        }
    }
}