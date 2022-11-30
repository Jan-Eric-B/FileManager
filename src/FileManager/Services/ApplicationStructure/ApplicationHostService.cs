using FileManager.Views;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Mvvm.Contracts;

namespace FileManager.Services.ApplicationStructure
{
    /// <summary>
    /// Managed host of the application.
    /// </summary>
    public class ApplicationHostService : IHostedService
    {
        private readonly INavigationService _navigationService;
        private readonly IPageService _pageService;
        private readonly IServiceProvider _serviceProvider;
        private INavigationWindow _navigationWindow;

        /// <summary>
        /// Beginning of loading the application.
        /// </summary>
        public ApplicationHostService(IServiceProvider serviceProvider, INavigationService navigationService, IPageService pageService)
        {
            _serviceProvider = serviceProvider;
            _navigationService = navigationService;
            _pageService = pageService;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            PrepareNavigation();

            await HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private async Task HandleActivationAsync()
        {
            await Task.CompletedTask;

            if (!Application.Current.Windows.OfType<Container>().Any())
            {
                _navigationWindow = _serviceProvider.GetService(typeof(INavigationWindow)) as INavigationWindow;
                _navigationWindow!.ShowWindow();
                _navigationWindow.Navigate(typeof(Views.Pages.GeneralPage));
            }

            await Task.CompletedTask;
        }

        private void PrepareNavigation()
        {
            _navigationService.SetPageService(_pageService);
        }
    }
}