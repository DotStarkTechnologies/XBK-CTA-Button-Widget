using CMS.Core;

using Kentico.Components.Web.Mvc.FormComponents;
using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

using DotStark.XBK.CTAButtonWidget;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using CMS.Helpers;

[assembly: RegisterWidget(
    identifier: CTAButtonWidgetViewComponent.IDENTIFIER, typeof(CTAButtonWidgetViewComponent),
    name: "CTA Button",
    propertiesType: typeof(CTAButtonWidgetProperties),
    Description = "Call to action button",
    IconClass = "icon-paperclip",
    AllowCache = true)]

namespace DotStark.XBK.CTAButtonWidget;

/// <summary>
/// Class which constructs the <see cref="CTAButtonWidgetViewModel"/> and renders the widget.
/// </summary>
public class CTAButtonWidgetViewComponent : ViewComponent
{
    /// <summary>
    /// The internal identifier of the CTAButton widget.
    /// </summary>
    public const string IDENTIFIER = "DotStark.XBK.Widget.CTAButton";

    private readonly IEventLogService eventLogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CTAButtonWidgetViewComponent"/> class.
    /// </summary>
    public CTAButtonWidgetViewComponent(IEventLogService eventLogService)
    {
        this.eventLogService = eventLogService;
    }

    /// <summary>
    /// Populates the <see cref="CTAButtonWidgetViewModel"/> and returns the appropriate view.
    /// </summary>
    /// <param name="widgetProperties">User populated properties from the page builder or view.</param>
    public IViewComponentResult Invoke(ComponentViewModel<CTAButtonWidgetProperties> widgetProperties)
    {
        if (widgetProperties is null)
        {
            LogWidgetLoadError("Widget properties were not provided.");
            return Content(String.Empty);
        }

        var vm = new CTAButtonWidgetViewModel
        {
            IsVisible = widgetProperties.Properties.IsVisible,
            Text = ValidationHelper.GetString(widgetProperties.Properties?.Text, ""),
            LinkUrl = ValidationHelper.GetString(widgetProperties.Properties?.LinkUrl, "#"),
            Alignment = ValidationHelper.GetString($"text-{widgetProperties.Properties?.Alignment}", ""),
            ButtonTarget = widgetProperties.Properties.OpenInNewTab ? "target=_blank" : "",
            CssClass = ValidationHelper.GetString(widgetProperties.Properties?.CssClass, "")
        };

        return View("~/Components/Widgets/CTAButtonWidget/_CTAButtonWidget.cshtml", vm);
    }

    private void LogWidgetLoadError(string description)
    {
        eventLogService.LogError("CTA Button Widget",
                "Load",
                description,
                new LoggingPolicy(TimeSpan.FromMinutes(1)));
    }
}

/// <summary>
/// The configurable properties for the CTAButton widget.
/// </summary>
public class CTAButtonWidgetProperties : IWidgetProperties
{
    /// <summary>
    /// Indicates if widget is Visible on live site or not.
    /// </summary>
    [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 0, Label = "Visible", DefaultValue = true)]
    public bool IsVisible
    {
        get;
        set;
    }

    /// <summary>
    /// The text of the button.
    /// </summary>
    [EditingComponent(TextInputComponent.IDENTIFIER, Order = 1, Label = "Button text")]
    [Required(ErrorMessage = "The 'Button Text' is required.")]
    public string Text
    {
        get;
        set;
    }

    /// <summary>
    /// The URL where the button points to.
    /// </summary>
    [EditingComponent(UrlSelector.IDENTIFIER, Order = 2, Label = "Link Url")]
    [EditingComponentProperty(nameof(UrlSelectorProperties.Placeholder), "Please enter a URL or select a page...")]
    [EditingComponentProperty(nameof(UrlSelectorProperties.Tabs), ContentSelectorTabs.Page)]
    public string LinkUrl
    {
        get;
        set;
    }

    /// <summary>
    /// The alignment of the button.
    /// </summary>
    [EditingComponent(DropDownComponent.IDENTIFIER, Order = 3, Label = "Alignment")]
    [EditingComponentProperty(nameof(DropDownProperties.DataSource), "start;Left\r\ncenter;Center\r\nend;Right")]
    public string Alignment
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates if link should be opened in a new tab.
    /// </summary>
    [EditingComponent(CheckBoxComponent.IDENTIFIER, Order = 4, Label = "Open in a new tab")]
    public bool OpenInNewTab
    {
        get;
        set;
    }

    /// <summary>
    /// The CSS class(es) added to the CTA Button widget's containing DIV.
    /// </summary>
    [EditingComponent(TextInputComponent.IDENTIFIER, Order = 5, Label = "Css classes", ExplanationText = "Enter any number of CSS classes to apply to the CTA Button, e.g. 'cta-button'")]
    public string CssClass
    {
        get;
        set;
    } = "cta-button";
}

/// <summary>
/// The properties to be set when rendering the widget on a view.
/// </summary>
public class CTAButtonWidgetViewModel
{
    /// <summary>
    /// Indicates if widget is Visible on live site or not.
    /// </summary>
    public bool IsVisible
    {
        get;
        set;
    }

    /// <summary>
    /// The text of the button.
    /// </summary>
    public string Text
    {
        get;
        set;
    }

    /// <summary>
    /// The URL where the button points to.
    /// </summary>
    public string LinkUrl
    {
        get;
        set;
    }

    /// <summary>
    /// The alignment of the button.
    /// </summary>
    public string Alignment
    {
        get;
        set;
    }

    /// <summary>
    /// Indicates if link should be opened in a new tab.
    /// </summary>
    public string ButtonTarget
    {
        get;
        set;
    }

    /// <summary>
    /// The Custom Css Class of the button.
    /// </summary>
    public string CssClass
    {
        get;
        set;
    }
}