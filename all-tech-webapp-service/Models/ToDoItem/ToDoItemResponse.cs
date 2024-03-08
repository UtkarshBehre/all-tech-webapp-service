﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace all_tech_webapp_service.Models.ToDoItem
{
    public class ToDoItemResponse
    {
        [JsonProperty("taskTitle")]
        public required string TaskTitle { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("isComplete")]
        public bool IsComplete { get; set; }
    }
}