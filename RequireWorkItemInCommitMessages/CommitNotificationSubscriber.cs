// -----------------------------------------------------------------------
// <copyright file="CommitNotificationSubscriber.cs" company="Enterprise Products Partners L.P. (Enterprise)">
// © Copyright 2012 - 2017, Enterprise Products Partners L.P. (Enterprise), All Rights Reserved.
// Permission to use, copy, modify, or distribute this software source code, binaries or
// related documentation, is strictly prohibited, without written consent from Enterprise.
// For inquiries about the software, contact Enterprise: Enterprise Products Company Law
// Department, 1100 Louisiana, 10th Floor, Houston, Texas 77002, phone 713-381-6500.
// </copyright>
// -----------------------------------------------------------------------

using System;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.Git.Server;

// See http://almsports.net/tfs-server-side-check-in-policy-for-git-repositories/1025/
namespace RequireWorkItemInCommitMessages
{
    public class CommitNotificationSubscriber : ISubscriber
    {
        public Type[] SubscribedTypes()
        {
            return new Type[1] {typeof(PushNotification)};
        }

        public EventNotificationStatus ProcessEvent(IVssRequestContext requestContext, NotificationType notificationType,
            object notificationEventArgs, out int statusCode, out string statusMessage,
            out ExceptionPropertyCollection properties)
        {
            statusCode = 0;
            statusMessage = string.Empty;
            properties = null;

            if (notificationType == NotificationType.DecisionPoint && notificationEventArgs is PushNotification)
            {
                PushNotification pushNotification = notificationEventArgs as PushNotification;

                var repositoryService = requestContext.GetService<ITeamFoundationGitRepositoryService>();

                using (
                    ITfsGitRepository gitRepository = repositoryService.FindRepositoryById(requestContext,
                        pushNotification.RepositoryId))
                {
                    foreach (Sha1Id item in pushNotification.IncludedCommits)
                    {
                        TfsGitCommit gitCommit = (TfsGitCommit) gitRepository.LookupObject(item);

                        var comment = gitCommit.GetComment(requestContext);

                        if (!CommitRules.IsCommitAcceptable(comment))
                        {
                            statusMessage = $"Non-merge commits must contain links to TFS (i.e. #12345) [Repository Name: {gitRepository.Name}].";
                            return EventNotificationStatus.ActionDenied;
                        }
                    }
                }
            }

            return EventNotificationStatus.ActionApproved;
        }

        public string Name => "Commit Notification Subscriber";
        public SubscriberPriority Priority => SubscriberPriority.Normal;
    }
}