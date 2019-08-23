using AnvilBank.Common;
using AnvilBank.Common.EmailSender;
using AnvilBank.Data;
using AnvilBank.Models;
using AnvilBank.Services.Contracts;
using AnvilBank.Services.Models.Transaction;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnvilBank.Services.Implementations
{
    public class TransactionService : BaseService, ITransactionService
    {
        private readonly IEmailSender emailSender;

        public TransactionService(AnvilBankDbContext context, IEmailSender emailSender)
            : base(context)
        {
            this.emailSender = emailSender;
        }

        public async Task<IEnumerable<T>> GetTransactionAsync<T>(string referenceNumber)
            where T : TransactionBaseServiceModel
            => await this.context
                .Transactions
                .Where(t => t.ReferenceNumber == referenceNumber)
                .ProjectTo<T>()
                .ToArrayAsync();

        public async Task<IEnumerable<T>> GetTransactionsAsync<T>(string userId, int pageIndex = 1, int count = int.MaxValue)
            where T : TransactionBaseServiceModel
            => await this.context
                .Transactions
                .Where(t => t.Account.UserId == userId)
                .OrderByDescending(mt => mt.MadeOn)
                .Skip((pageIndex - 1) * count)
                .Take(count)
                .ProjectTo<T>()
                .ToArrayAsync();

        public async Task<bool> CreateTransactionAsync<T>(T model)
            where T : TransactionBaseServiceModel
        {
            if (!this.IsEntityStateValid(model))
            {
                return false;
            }

            var dbModel = Mapper.Map<Transaction>(model);
            var userAccount = await this.context
                .Accounts
                .Include(u => u.User)
                .Where(u => u.Id == dbModel.AccountId)
                .SingleOrDefaultAsync();
            if (userAccount == null)
            {
                return false;
            }

            userAccount.Balance += dbModel.Amount;
            this.context.Update(userAccount);

            await this.context.Transactions.AddAsync(dbModel);
            await this.context.SaveChangesAsync();

            if (dbModel.Amount > 0)
            {
                await this.emailSender.SendEmailAsync(dbModel.Account.User.Email, EmailMessages.ReceiveMoneySubject,
                    string.Format(EmailMessages.ReceiveMoneyMessage, dbModel.Amount));
            }
            else
            {
                await this.emailSender.SendEmailAsync(dbModel.Account.User.Email, EmailMessages.SendMoneySubject,
                    string.Format(EmailMessages.SendMoneyMessage, Math.Abs(dbModel.Amount)));
            }

            return true;
        }

        public async Task<int> GetCountOfAllTransactionsForUserAsync(string userId)
            => await this.context
                .Transactions
                .CountAsync(t => t.Account.UserId == userId);

        public async Task<int> GetCountOfAllTransactionsForAccountAsync(string accountId)
            => await this.context
                .Transactions
                .CountAsync(t => t.AccountId == accountId);


        public async Task<IEnumerable<T>> GetLast10TransactionsForUserAsync<T>(string userId)
            where T : TransactionBaseServiceModel
            => await this.context
                .Transactions
                .Where(mt => mt.Account.UserId == userId)
                .OrderByDescending(mt => mt.MadeOn)
                .Take(10)
                .ProjectTo<T>()
                .ToArrayAsync();

        public async Task<IEnumerable<T>> GetTransactionsForAccountAsync<T>(string accountId, int pageIndex = 1, int count = int.MaxValue)
            where T : TransactionBaseServiceModel
            => await this.context
                .Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(mt => mt.MadeOn)
                .Skip((pageIndex - 1) * count)
                .Take(count)
                .ProjectTo<T>()
                .ToArrayAsync();
    }
}
