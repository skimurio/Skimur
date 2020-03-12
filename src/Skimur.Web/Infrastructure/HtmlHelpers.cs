﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Skimur.Web.ViewModels;
using Skimur.Data;
using Microsoft.AspNetCore.Html;

namespace Skimur.Web.Infrastructure
{
    public static class HtmlHelpers
    {
        public static HtmlString ErrorMessages(this IHtmlHelper helper)
        {
            List<string> messages = null;

            var errorMessagesArray = helper.ViewContext.TempData["ErrorMessages"] as string[];

            if (errorMessagesArray != null && errorMessagesArray.Length > 0)
            {
                if (messages == null)
                    messages = new List<string>();
                foreach (var message in errorMessagesArray)
                    messages.Add(message);
            }

            var errorMessagesList = helper.ViewContext.TempData["ErrorMessages"] as List<string>;

            if (errorMessagesList != null && errorMessagesList.Count > 0)
            {
                if (messages == null)
                    messages = new List<string>();
                foreach (var message in errorMessagesList)
                    messages.Add(message);
            }

            if (!helper.ViewData.ModelState.IsValid)
            {
                var modelErrors = helper.ViewData.ModelState[string.Empty];
                if (modelErrors != null && modelErrors.Errors.Count > 0)
                {
                    if (messages == null)
                        messages = new List<string>();
                    messages.AddRange(modelErrors.Errors.Select(modelError => modelError.ErrorMessage));
                }
            }

            if (messages == null || messages.Count == 0) return HtmlString.Empty;

            var html = new StringBuilder();
            html.Append("<div class=\"validation-summary-errors alert alert-danger\"><ul>");
            foreach (var message in messages)
            {
                html.Append("<li>" + message + "</li>");
            }
            html.Append("</ul></div>");

            return new HtmlString(html.ToString());
        }

        public static HtmlString SuccessMessages(this IHtmlHelper helper)
        {
            List<string> messages = null;

            var successMessagesArray = helper.ViewContext.TempData["SuccessMessages"] as string[];

            if (successMessagesArray != null && successMessagesArray.Length > 0)
            {
                if (messages == null)
                    messages = new List<string>();
                foreach (var message in successMessagesArray)
                    messages.Add(message);
            }

            var successMessagesList = helper.ViewContext.TempData["SuccessMessages"] as List<string>;

            if (successMessagesList != null && successMessagesList.Count > 0)
            {
                if (messages == null)
                    messages = new List<string>();
                foreach (var message in successMessagesList)
                    messages.Add(message);
            }

            if (messages == null)
                return HtmlString.Empty;

            if (messages.Count == 0) return HtmlString.Empty;

            var html = new StringBuilder();
            html.Append("<div class=\"validation-summary-infos alert alert-info\"><ul>");
            foreach (var message in messages)
            {
                html.Append("<li>" + message + "</li>");
            }
            html.Append("</ul></div>");

            return new HtmlString(html.ToString());
        }
    }
}
