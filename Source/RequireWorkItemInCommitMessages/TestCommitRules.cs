// -----------------------------------------------------------------------
// <copyright file="TestCommitRules.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2017, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// -----------------------------------------------------------------------

using FluentAssertions;

using NUnit.Framework;

namespace RequireWorkItemsInCommitMessages
{
    [TestFixture]
    class TestCommitRules
    {
        [Test]
        public void Example1()
        {
            CommitRules.IsCommitAcceptable("#104581: requested to show Processing date and Last Run date on DVR Detail screen").Should().BeTrue();
        }

        [Test]
        public void Example2()
        {
            CommitRules.IsCommitAcceptable("#104752 - reset settings files.").Should().BeTrue();
        }

        [Test]
        public void Example3()
        {
            CommitRules.IsCommitAcceptable("Labs package update #104488 #104761").Should().BeTrue();
        }

        [Test]
        public void Example4()
        {
            CommitRules.IsCommitAcceptable("Related Work Items: #2844").Should().BeTrue();
        }

        [Test]
        public void BadExample1()
        {
            CommitRules.IsCommitAcceptable("Merge in develop").Should().BeFalse();
        }

        [Test]
        public void BadExample2()
        {
            CommitRules.IsCommitAcceptable("Fixed migration and merge from release branch").Should().BeFalse();
        }
        [Test]
        public void BadExample3()
        {
            CommitRules.IsCommitAcceptable("Remonved an unnecessary file from my prior commit.").Should().BeFalse();
        }

        [Test]
        public void Merge1()
        {
            CommitRules.IsCommitAcceptable("    Merge branch \'r-ethane-systems\' into f-NominationHeaderLinkSecurity").Should().BeTrue();
        }

        [Test]
        public void Merge2()
        {
            CommitRules.IsCommitAcceptable("Merge remote-tracking branch \'origin/develop\' into mmb-0322-release-2-develop").Should().BeTrue();
        }

        [Test]
        public void Merge3()
        {
            CommitRules.IsCommitAcceptable("Merge branch 'r-ethane-systems' into mmb-0322-release").Should().BeTrue();
        }
    }
}
