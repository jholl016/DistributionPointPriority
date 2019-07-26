# Distribution Point Priority Manager
An app for altering ConfigMgr Distribution Point content distribution priority values
by dragging and dropping on a list to reorder the servers.
     
These priority values are only exposed via WMI on the site server and are not visible
or editable in the ConfigMgr admin console. For a full description of how these priorities
work, see this article:
https://techcommunity.microsoft.com/t5/Configuration-Manager-Archive/Content-Distribution-Priorities/ba-p/273393

This software is provided "as-is". No claim of suitability, guarantee, or any warranty whatsoever is provided.

### Instructions
- There is no mechanism for entering credentials. You must run the EXE as a user with admin rights on the ConfigMgr site server.
- Enter the FQDN of the site server and the ConfigMgr site code, then click **Get Priorities** to retrieve the current settings for all distribution points in the specified site.
- Drag'n'drop to reorder servers in the list. New priority values will be calculated automatically.
- The **Use Shared Priority** checkboxes determine whether the distribution point should share a priority value with adjacent list items.
- The **Reset All** button will set all distribution points to use shared priority and assign the default priority value of 200.
- When you are satisfied with the new priority values shown in the list, use the **Save Changes** button to write those values back to WMI on the site server.

### Download
The full source code and VS2019 solution is provided. If you only want to download the compiled EXE, it's in the release folder (https://github.com/jholl016/DistributionPointPriority/tree/master/Distribution%20Point%20Priority/bin/Release).
     
### Attributions
ListViewDragDropManager and associated classes by Josh Smith. Used in accordance with the Code Project Open License. (https://www.codeproject.com/Articles/17266/Drag-and-Drop-Items-in-a-WPF-ListView)
