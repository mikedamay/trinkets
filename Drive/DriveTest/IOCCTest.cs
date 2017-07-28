﻿using System;
using com.TheDisappointedProgrammer.Drive;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IOCCTest
{
    [TestClass]
    public class IOCCTest
    {
        [TestMethod]
        public void SelfTest()
        {
            IOCC.Instance.GetOrCreateObjectTree<SheetProcessor>();
            Assert.IsFalse(false);
        }

        [TestMethod]
        public void ShouldBuildTreeFromWellFormedFields()
        {
            TreeWithFields twf 
              = IOCC.Instance.GetOrCreateObjectTree<TreeWithFields>();
            Assert.AreNotEqual(null, twf.childOne);
        }
    }
}
