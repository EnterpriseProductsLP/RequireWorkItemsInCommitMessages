// -----------------------------------------------------------------------
// <copyright file="CommitRules.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2017, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace RequireWorkItemsInCommitMessages
{
    public class CommitRules
    {
        private static bool HasWorkItem(string comment)
        {
            var matches = Regex.Matches(comment, "\\#[0-9]*");
            return (matches.Count > 0);
        }

        private static bool IsMerge(string comment)
        {
            comment = comment.Trim();

            return comment.StartsWith("Merge branch ") || comment.StartsWith("Merge remote-tracking branch ");
        }

        public static bool IsCommitAcceptable(string comment)
        {
            return HasWorkItem(comment) || IsMerge(comment);
        }
    }
}