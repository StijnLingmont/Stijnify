using Autofac;
using MediaManager.Player;
using stijnify.Interfaces;
using stijnify.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify
{
    class Container
    {
        public static IContainer ContainerInstance { get; set; }

        public static void Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(new MediaPlayerService()).As<IMediaPlayerService>();
            builder.RegisterType<MediaPlayerService>();

            var test = builder.Build();

            ContainerInstance = test;
        }
    }
}
