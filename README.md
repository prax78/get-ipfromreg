# get-ipfromreg
Get-IPfromReg -computername "servername"
this will fetch IPaddress,subnet and geteway info from remote server registry
How to run this
Open POwershell with admininstrator rights
import-module getipfromreg.dll
get-IpfromReg -Computername "servername"
output will be server IP,subnetmask,gateway and servername
