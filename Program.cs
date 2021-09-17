using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore
{
    class Program
    {
        static void Main(string[] args)
        {
            // aqui vou simular a entrada de uma compra

            var livro01 = new Book() { Title = "Livro 01", Price = new decimal(8.00), Quantity = 2 };
            var livro02 = new Book() { Title = "Livro 02", Price = new decimal(8.00), Quantity = 2 };
            var livro03 = new Book() { Title = "Livro 03", Price = new decimal(8.00), Quantity = 2 };
            var livro04 = new Book() { Title = "Livro 04", Price = new decimal(8.00), Quantity = 1 };
            var livro05 = new Book() { Title = "Livro 05", Price = new decimal(8.00), Quantity = 1 };

            // criando uma lista ou array para armazenar os itens comprados.
            List<Book> livros = new();
            livros.Add(livro01);
            livros.Add(livro02);
            livros.Add(livro03);
            livros.Add(livro04);
            livros.Add(livro05);



            Discount discount = new Discount();
            List<List<Discount>> listGroupDiscount = new();
            listGroupDiscount = discount.CalculateDiscount(livros);



            PrintResult(GroupMinAmount(listGroupDiscount)); //Resultado

          

            static decimal MinAmountGroup(List<List<Discount>> listGroupDiscount)
            {
                List<decimal> AmountGroup = new();

                foreach (var group in listGroupDiscount)
                {
                    var minAmount = group.Sum(x => x.Amount);
                    AmountGroup.Add(minAmount);
                }
                return AmountGroup.Min();
            }


            static List<Discount> GroupMinAmount(List<List<Discount>> GroupDiscount)
            {
                var minAmount = MinAmountGroup(GroupDiscount);
                List<Discount> AmountMinGroup = new();

                foreach (var group in GroupDiscount)
                {

                    if (group.Sum(s=> s.Amount) == minAmount)
                    {
                       return group;
                    }
                }
                return null;
            }





            static void PrintResult(List<Discount> GroupDiscount)
            {
                StringBuilder sb = new();
                int groupNumber = 1;
                decimal total = new();


                sb.AppendLine("Livros comprados: \n");
                foreach (var group in GroupDiscount )
                {
                    total += group.Amount;
                    sb.AppendLine($"Grupo - {groupNumber}");
                    var groupPrint = group.BookGroup.Select(s => new { titles = s.Title, price = s.Price, quantity = s.Quantity}).ToList();

                    int groupPrintCount = groupPrint.Count;
                  
                    for (int i = 0; i < groupPrintCount; i++)
                    {
                        sb.Append($"Título: {groupPrint[i].titles} | ");
                        sb.Append($"Quantidade: {groupPrint[i].quantity} | ");
                        sb.AppendLine($"Preço: {groupPrint[i].price}");                       
                    }
                    sb.AppendLine();
                    sb.AppendLine($"Percentual de desconto: {group.DiscountValue}%");
                    sb.AppendLine($"Valor sem desconto: {group.BookGroup.Sum(s => s.Price).ToString("0.00")}");
                    sb.AppendLine($"Valor com desconto: {group.Amount.ToString("0.00")}");
                    sb.AppendLine("_______________________________________________\n");
                    groupNumber++;
                }

                sb.AppendLine();
                sb.AppendLine($"Valor total: {total.ToString("0.00")}");

                Console.WriteLine(sb.ToString());
            }

           

        }
    }
}
