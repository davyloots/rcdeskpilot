// requires Windows 2000 Service Pack 3, Windows 98, Windows 98 Second Edition, Windows ME, Windows Server 2003, Windows XP Service Pack 2
// requires internet explorer 5.0.1 or higher
// requires windows installer 2.0 on windows 98, ME
// requires windows installer 3.1 on windows 2000 or higher
// http://www.microsoft.com/downloads/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5

[CustomMessages]
directx9_title=DirectX 9.0c

en.directx90c_size=1 MB
//de.directx90c_size=1 MB


[Code]
const
	directx9_url = 'http://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe';
	directx9_url_x64 = 'http://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe';
	directx9_url_ia64 = 'http://download.microsoft.com/download/1/7/1/1718CCC4-6315-4D8E-9543-8E28A4E18C4C/dxwebsetup.exe';

procedure directx90c(MinVersion: string);
var
	version: string;
begin
	RegQueryStringValue(HKEY_LOCAL_MACHINE, 'Software\Microsoft\DirectX\', 'Version', version);
	if version < MinVersion then begin
		AddProduct('dxwebsetup.exe',
			'',
			CustomMessage('directx9_title'),
			CustomMessage('directx90c_size'),
			GetURL(directx9_url, directx9_url, directx9_url));
	end;
end;

procedure manageddirectx90c();
var
	filename: string;
begin
	filename := ExpandConstant('{win}\Microsoft.NET\DirectX for Managed Code\1.0.2911.0\Microsoft.DirectX.Direct3DX.dll');
	if FileExists(filename)
	then
	else
	begin
		AddProduct('dxwebsetup.exe',
			'',
			CustomMessage('directx9_title'),
			CustomMessage('directx90c_size'),
			GetURL(directx9_url, directx9_url, directx9_url));
	end;
end;
