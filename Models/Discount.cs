using System.Collections.Generic;
using System.Linq;

namespace BookStore.Models
{
    public class Discount
    {
        public IEnumerable<Book> BookGroup { get; set; } // grupo de livros distintos que formam um grupo para desconto.
        public double DiscountValue { get; set; } // valor da porcentagem de desconto.
        public decimal Amount { get; set; } // valor do grupo com desconto.

        

        public List<List<Discount>> CalculateDiscount(List<Book> listBooks)
        {
            int distinctBooks = listBooks.Where(x => x.Title != null).Distinct().Count();
            List<List<Discount>> DiscountList = new(); // varias combinações para achar o menor custo;


            while (distinctBooks > 1)
            {
                DiscountList.Add(GetGroupsDiscount(listBooks, distinctBooks));
                distinctBooks--;
            }
            return DiscountList;
        }
        public List<Discount> GetGroupsDiscount(List<Book> listBooks, int combination)
        {
            List<Discount> discountGroups = new();
            bool groupExist = true;

            while (groupExist)
            {
                BookGroup = DistinctBooks(listBooks, combination);

                DiscountValue = GetDiscountGroup(BookGroup);

                discountGroups.Add(new Discount { BookGroup = BookGroup, DiscountValue = DiscountValue });

                RemoveDistinctBooksGroup(ref listBooks, BookGroup);

                groupExist = VerifyGroupExist(listBooks);
            }

            GetAmountWithDiscount(discountGroups);

            return discountGroups;
        }

        private IEnumerable<Book> DistinctBooks(List<Book> listBooks, int combination)
        {
            var distinctBooks = listBooks.Where(l => l.Quantity > 0).Distinct().Take(combination);
            List<Book> books = new();            
            foreach (var item in distinctBooks)
            {
                Book itemBook = new();
                itemBook = item;
                if (item.Quantity > 1)
                {
                    itemBook.Quantity = 1;
                }
                books.Add(itemBook);
            }
            return books;
        }

        private List<Book> RemoveDistinctBooksGroup(ref List<Book> listBooks, IEnumerable<Book> BookGroup)
        {
            List<Book> books = new();
            var x = BookGroup.Select(s => s.Title).First();

            foreach (var book in listBooks)
            {
                var newBook = book;
                if (book.Quantity > 0 && BookGroup.Select(s => s.Title).Contains(book.Title))
                {
                    newBook.Quantity -= 1;
                }
                books.Add(newBook);
            }
           
            listBooks = books;

            return listBooks;
        }

        private double GetDiscountGroup(IEnumerable<Book> distinctBooks)
        {
            int numberBooks = distinctBooks.Count();
            double discount = 0;

            switch (numberBooks)
            {
                case 1:
                    discount = 0;
                    break;
                case 2:
                    discount = 5;
                    break;
                case 3:
                    discount = 10;
                    break;
                case 4:
                    discount = 20;
                    break;
                case 5:
                    discount = 25;
                    break;
            }
            return discount;
        }

        private IEnumerable<Discount> GetAmountWithDiscount(IEnumerable<Discount> discountGroups)
        {
            decimal sumAmount = new();
            double discountPercentage = 0.0;
            foreach (var group in discountGroups)
            {
                sumAmount = group.BookGroup.Sum(v => v.Price);
                discountPercentage = group.DiscountValue;

                group.Amount = sumAmount - (sumAmount * (decimal)discountPercentage / 100);
            }
            return discountGroups;
        }

        private bool VerifyGroupExist(List<Book> listBooks)
        {
            foreach (var item in listBooks)
            {
                if (item.Quantity > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
