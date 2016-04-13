﻿using System;
using System.Collections.Generic;
using Confifu.Abstractions;

namespace Confifu
{
    public class AppSetup
    {
        public const string CSharpEnvKey = "CSHARP_ENV";

        protected IEnvironmentVariables Env { get;}

        private readonly AppConfig _appConfig;
        protected IAppConfig AppConfig => _appConfig;

        private List<AppSetupAction> SetupActions { get; } = new List<AppSetupAction>();
        private string CSharpEnv => Env[CSharpEnvKey];

        protected AppSetup(IEnvironmentVariables env)
        {
            Env = env;
            _appConfig = new AppConfig();
            _appConfig.SetEnvironmentVariables(env);
        }

        public void Setup()
        {
            var csharpEnv = CSharpEnv;
            foreach (var setupAction in SetupActions)
            {
                if (setupAction.Environment == null
                    || string.Compare(setupAction.Environment, csharpEnv,
                        StringComparison.InvariantCultureIgnoreCase) == 0)
                    setupAction.Action();
            }
            _appConfig.MarkSetupComplete();
        }

        protected void Common(Action action)
        {
            SetupActions.Add(new AppSetupAction(action));
        }

        protected void Environment(string environment, Action action)
        {
            SetupActions.Add(new AppSetupAction(action, environment));
        }
    }
}