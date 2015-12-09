WS_CLOUDINARY
============

Fetch enables on-the-fly manipulation of existing remote images and optimized delivery via a CDN. Fetched images are cached on your Cloudinary account for performance reasons.

If you are using fetch feature to manipulate image on the fly but do not want to store any images in cloudinary cloud then you can use this windows service to bulk delete the images.

#C# Windows Service to delete Cloudinary Resources

1.  Rename App.config.rename to App.config
2.  Open App.config and update with log path for log4net appenders
3.  Setup Timer_INTERVAL, Cloudname, API KEY, API Secret
4.  Install using Installutil command ( Installutil Cloudinary_WService.exe)


