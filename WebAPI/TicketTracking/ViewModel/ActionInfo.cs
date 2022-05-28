using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Shared.ViewModels
{
    public class ActionType
    {
        public const string Search = "Search";
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";
    }

    public enum ActionStatus
    {
        Success = 0,
        Fail = 1,
    }
    [Serializable]
    public class ActionInfo
    {
        /// <summary>
        /// True or False.
        /// </summary>
        /// <value>Start.</value>
        [DataMember(Name = "Status", EmitDefaultValue = false)]
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// reulst of message for show.
        /// </summary>
        [Required]
        public string Message { get; set; }
        /// <summary>
        /// reulst of data.
        /// </summary>
        public object Data { get; set; }
        public ActionInfo()
        {

            Status = ActionStatus.Fail.ToString();
            Message = "";
        }
    }
}
