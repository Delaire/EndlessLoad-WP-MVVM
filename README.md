MVVM Endless load on jumplist for Windows Phone Apps (C#/XAML)
=========================
 
[![Build status](https://ci.appveyor.com/api/projects/status/eogo6j3gr7flioul?svg=true)](https://ci.appveyor.com/project/Delaire/endlessload-wp-mvvm)

look into the MainViewModel.cs file

 - in there is where all of the magic is!

this methode:

 private async Task LoadEverything(bool isRefresh)
        {
               DataListLoader = factory.GetIncrementalCollection<PostsEntity>(1,
                GetMainPageFeedLoader, //<- Method Querying
                () =>
                {
                    //<- Start Query
                }, 
                (data) =>
                {
                    // <- End Query
                }); 
        }


License

This project is licensed under the MIT license.
