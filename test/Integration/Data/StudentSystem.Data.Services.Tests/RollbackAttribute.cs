using System;
using System.Transactions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace StudentSystem.Data.Services.Tests
{
    public class RollbackAttribute : Attribute, ITestAction
    {
        private TransactionScope _transaction;

        public void BeforeTest(ITest test)
        {
            _transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public void AfterTest(ITest test)
        {
            _transaction.Dispose();
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}