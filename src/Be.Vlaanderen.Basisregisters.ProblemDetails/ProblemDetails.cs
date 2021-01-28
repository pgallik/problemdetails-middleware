namespace Be.Vlaanderen.Basisregisters.BasicApiProblem
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>Implementation of Problem Details for HTTP APIs https://tools.ietf.org/html/rfc7807</summary>
    ///// <summary>A machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807.</summary>
    [DataContract(Name = "ProblemDetails", Namespace = "")]
    public class ProblemDetails
    {
        public static string DefaultTitle { get; } = "Er heeft zich een fout voorgedaan!"; // TODO: Localize

        public static string GetProblemNumber() => $"{Guid.NewGuid():N}";

        public static string GetTypeUriFor<T>(T _) where T : Exception
            => GetTypeUriFor<T>();

        public static string GetTypeUriFor<T>() where T : Exception
            => GetTypeUriFor<T>(Assembly.GetEntryAssembly()?.GetName().Name ?? "problem-details-undefined-namespace");

        public static string GetTypeUriFor<T>(T _, string customNamespace) where T : Exception
            => GetTypeUriFor<T>(customNamespace);

        public static string GetTypeUriFor<T>(string customNamespace) where T : Exception
        {
            if (string.IsNullOrWhiteSpace(customNamespace))
                throw new ArgumentNullException(nameof(customNamespace));

            var name = typeof(T).Name.Replace("Exception", string.Empty);
            if (string.IsNullOrWhiteSpace(name))
                name = "Unknown";

            return $"urn:{customNamespace}:{name}".ToLowerInvariant();
        }

        /// <summary>URI referentie die het probleem type bepaalt.</summary>
        ///// <summary>A URI reference [RFC3986] that identifies the problem type. This specification encourages that, when dereferenced, it provide human-readable documentation for the problem type (e.g., using HTML [W3C.REC-html5-20141028]). When this member is not present, its value is assumed to be "about:blank".</summary>
        [JsonProperty("type", Required = Required.DisallowNull)]
        [DataMember(Name = "Type", Order = 100, EmitDefaultValue = false)]
        [Description("URI referentie die het probleem type bepaalt.")]
        public string ProblemTypeUri { get; set; }

        /// <summary>Korte omschrijving van het probleem.</summary>
        ///// <summary>A short, human-readable summary of the problem type.It SHOULD NOT change from occurrence to occurrence of the problem, except for purposes of localization(e.g., using proactive content negotiation; see[RFC7231], Section 3.4).</summary>
        [JsonProperty("title", Required = Required.DisallowNull)]
        [DataMember(Name = "Title", Order = 200, EmitDefaultValue = false)]
        [Description("Korte omschrijving van het probleem.")]
        public string Title { get; set; }

        /// <summary>Specifieke details voor dit probleem.</summary>
        ///// <summary>A human-readable explanation specific to this occurrence of the problem.</summary>
        [JsonProperty("detail", Required = Required.DisallowNull)]
        [DataMember(Name = "Detail", Order = 300, EmitDefaultValue = false)]
        [Description("Specifieke details voor dit probleem.")]
        public string Detail { get; set; }

        /// <summary>HTTP status code komende van de server voor dit probleem.</summary>
        ///// <summary>The HTTP status code([RFC7231], Section 6) generated by the origin server for this occurrence of the problem.</summary>
        [JsonProperty("status", Required = Required.DisallowNull)]
        [DataMember(Name = "Status", Order = 400, EmitDefaultValue = false)]
        [Description("HTTP status code komende van de server voor dit probleem.")]
        public int? HttpStatus { get; set; }

        /// <summary>URI naar de specifieke instantie van dit probleem.</summary>
        ///// <summary>A URI reference that identifies the specific occurrence of the problem.It may or may not yield further information if dereferenced.</summary>
        [JsonProperty("instance", Required = Required.DisallowNull)]
        [DataMember(Name = "Instance", Order = 500, EmitDefaultValue = false)]
        [Description("URI naar de specifieke instantie van dit probleem.")]
        public string ProblemInstanceUri { get; set; }
    }
}
