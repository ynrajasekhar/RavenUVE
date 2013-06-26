using System;
using System.ComponentModel;
using Common.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RavenUVE.ViewModel;

namespace RavenUVETest
{
    [TestClass]
    public class LoggingViewModelTest
    {

        #region Fields

        private Mock<ICollectionView> fakeCollectionView;
        private Mock<ILog> fakeLogger;
        private LoggingViewModel loggingViewModel;

        #endregion


        [TestInitialize]
        public void Setup()
        {
            fakeCollectionView = new Mock<ICollectionView>();
            fakeLogger = new Mock<ILog>();

            fakeCollectionView.SetupProperty(view => view.Filter);

            loggingViewModel = new LoggingViewModel(fakeLogger.Object, fakeCollectionView.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            fakeCollectionView = null;
            fakeLogger = null;
        }

        [TestMethod]
        public void LoggingViewModelConstructorTest()
        {
            Assert.IsNotNull(loggingViewModel);
            Assert.IsNotNull(fakeCollectionView.Object.Filter);
            Assert.IsNotNull(loggingViewModel.LogCollection);
        }
    }
}
