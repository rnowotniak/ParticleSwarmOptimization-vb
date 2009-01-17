function varargout = zad2(varargin)
% ZAD2 M-file for zad2.fig
%      ZAD2, by itself, creates a new ZAD2 or raises the existing
%      singleton*.
%
%      H = ZAD2 returns the handle to a new ZAD2 or the handle to
%      the existing singleton*.
%
%      ZAD2('CALLBACK',hObject,eventData,handles,...) calls the local
%      function named CALLBACK in ZAD2.M with the given input arguments.
%
%      ZAD2('Property','Value',...) creates a new ZAD2 or raises the
%      existing singleton*.  Starting from the left, property value pairs are
%      applied to the GUI before zad2_OpeningFunction gets called.  An
%      unrecognized property name or invalid value makes property application
%      stop.  All inputs are passed to zad2_OpeningFcn via varargin.
%
%      *See GUI Options on GUIDE's Tools menu.  Choose "GUI allows only one
%      instance to run (singleton)".
%
% See also: GUIDE, GUIDATA, GUIHANDLES

% Copyright 2002-2003 The MathWorks, Inc.

% Edit the above text to modify the response to help zad2

% Last Modified by GUIDE v2.5 07-Nov-2007 04:04:56

% Begin initialization code - DO NOT EDIT
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @zad2_OpeningFcn, ...
                   'gui_OutputFcn',  @zad2_OutputFcn, ...
                   'gui_LayoutFcn',  [] , ...
                   'gui_Callback',   []);
if nargin && ischar(varargin{1})
    gui_State.gui_Callback = str2func(varargin{1});
end

if nargout
    [varargout{1:nargout}] = gui_mainfcn(gui_State, varargin{:});
else
    gui_mainfcn(gui_State, varargin{:});
end
% End initialization code - DO NOT EDIT


% --- Executes just before zad2 is made visible.
function zad2_OpeningFcn(hObject, eventdata, handles, varargin)
% This function has no output args, see OutputFcn.
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
% varargin   command line arguments to zad2 (see VARARGIN)

% Choose default command line output for zad2
handles.output = hObject;

% Update handles structure
guidata(hObject, handles);

% UIWAIT makes zad2 wait for user response (see UIRESUME)
% uiwait(handles.figure1);


% --- Outputs from this function are returned to the command line.
function varargout = zad2_OutputFcn(hObject, eventdata, handles) 
% varargout  cell array for returning output args (see VARARGOUT);
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Get default command line output from handles structure
varargout{1} = handles.output;



function fitnessEdit_Callback(hObject, eventdata, handles)
% hObject    handle to fitnessEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of fitnessEdit as text
%        str2double(get(hObject,'String')) returns contents of fitnessEdit as a double


% --- Executes during object creation, after setting all properties.
function fitnessEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to fitnessEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end


% --- Executes on button press in drawButton.
function drawButton_Callback(hObject, eventdata, handles)
% hObject    handle to drawButton (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
%area(handles.axes1);
cla;
hold on;
xmin = str2double(get(handles.xminEdit, 'String'));
xmax = str2double(get(handles.xmaxEdit, 'String'));
ymin = str2double(get(handles.yminEdit, 'String'));
ymax = str2double(get(handles.ymaxEdit, 'String'));
precision = str2double(get(handles.precisionEdit, 'String'));
poplen = str2double(get(handles.poplenEdit, 'String'));
fitness = get(handles.fitnessEdit, 'String');
levels = str2double(get(handles.levelsEdit, 'String'));
ExchGen = str2double(get(handles.exchGenEdit, 'String'));
CalibGen = str2double(get(handles.calibGenEdit, 'String'));
HFC_Iterations = str2double(get(handles.iterationsEdit, 'String'));
Pc = str2double(get(handles.pcEdit, 'String'));
Pm = str2double(get(handles.pmEdit, 'String'));

%
% Draw function
%
domain = xmin:(xmax-xmin)/100:xmax;
codomain = ymin:(ymax-ymin)/100:ymax;
[x,y]=meshgrid(domain, codomain);
z=eval(fitness);
[contour1,h]=contour(x,y,z,20); 
datacursormode on;
axis([xmin xmax ymin ymax]);
set(h, 'LevelStepMode', 'manual');
 
if get(handles.tournamentRadioButton, 'Value')
    selection = 'tournament';
else
    selection = 'roulette';
end
if get(handles.binaryCoding, 'Value')
    coding = 'binary';
else
    coding = 'gray';
end
hfc;


function pcEdit_Callback(hObject, eventdata, handles)
% hObject    handle to pcEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of pcEdit as text
%        str2double(get(hObject,'String')) returns contents of pcEdit as a double


% --- Executes during object creation, after setting all properties.
function pcEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to pcEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function pmEdit_Callback(hObject, eventdata, handles)
% hObject    handle to pmEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of pmEdit as text
%        str2double(get(hObject,'String')) returns contents of pmEdit as a double


% --- Executes during object creation, after setting all properties.
function pmEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to pmEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function precisionEdit_Callback(hObject, eventdata, handles)
% hObject    handle to precisionEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of precisionEdit as text
%        str2double(get(hObject,'String')) returns contents of precisionEdit as a double


% --- Executes during object creation, after setting all properties.
function precisionEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to precisionEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function iterationsEdit_Callback(hObject, eventdata, handles)
% hObject    handle to iterationsEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of iterationsEdit as text
%        str2double(get(hObject,'String')) returns contents of iterationsEdit as a double


% --- Executes during object creation, after setting all properties.
function iterationsEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to iterationsEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function poplenEdit_Callback(hObject, eventdata, handles)
% hObject    handle to poplenEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of poplenEdit as text
%        str2double(get(hObject,'String')) returns contents of poplenEdit as a double


% --- Executes during object creation, after setting all properties.
function poplenEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to poplenEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function xminEdit_Callback(hObject, eventdata, handles)
% hObject    handle to xminEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of xminEdit as text
%        str2double(get(hObject,'String')) returns contents of xminEdit as a double


% --- Executes during object creation, after setting all properties.
function xminEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to xminEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function xmaxEdit_Callback(hObject, eventdata, handles)
% hObject    handle to xmaxEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of xmaxEdit as text
%        str2double(get(hObject,'String')) returns contents of xmaxEdit as a double


% --- Executes during object creation, after setting all properties.
function xmaxEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to xmaxEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function exchGenEdit_Callback(hObject, eventdata, handles)
% hObject    handle to exchGenEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of exchGenEdit as text
%        str2double(get(hObject,'String')) returns contents of exchGenEdit as a double


% --- Executes during object creation, after setting all properties.
function exchGenEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to exchGenEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function calibGenEdit_Callback(hObject, eventdata, handles)
% hObject    handle to calibGenEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of calibGenEdit as text
%        str2double(get(hObject,'String')) returns contents of calibGenEdit as a double


% --- Executes during object creation, after setting all properties.
function calibGenEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to calibGenEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function yminEdit_Callback(hObject, eventdata, handles)
% hObject    handle to yminEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of yminEdit as text
%        str2double(get(hObject,'String')) returns contents of yminEdit as a double


% --- Executes during object creation, after setting all properties.
function yminEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to yminEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function ymaxEdit_Callback(hObject, eventdata, handles)
% hObject    handle to ymaxEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of ymaxEdit as text
%        str2double(get(hObject,'String')) returns contents of ymaxEdit as a double


% --- Executes during object creation, after setting all properties.
function ymaxEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to ymaxEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end


% --- Executes on selection change in functionsPopupmenu.
function functionsPopupmenu_Callback(hObject, eventdata, handles)
% hObject    handle to functionsPopupmenu (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: contents = get(hObject,'String') returns functionsPopupmenu contents as cell array
%        contents{get(hObject,'Value')} returns selected item from functionsPopupmenu
popup_sel_index = get(handles.functionsPopupmenu, 'Value');
set(handles.xminEdit, 'String', '-5');
set(handles.xmaxEdit, 'String', '5');
set(handles.yminEdit, 'String', '-5');
set(handles.ymaxEdit, 'String', '5');
switch popup_sel_index
    case 1
        set(handles.fitnessEdit, 'String', '-(x.^2+y.^2)');
    case 2
        set(handles.fitnessEdit, 'String', '-(x.^2+2*y.^2)');
    case 3
        set(handles.fitnessEdit, 'String', '-(x.^2+x.^2+y.^2)');
    case 4
        set(handles.fitnessEdit, 'String', '-(5*x.^2 + 5*2*y.^2)');
    case 5
        set(handles.xminEdit, 'String', '-2');
        set(handles.xmaxEdit, 'String', '2');
        set(handles.yminEdit, 'String', '-2');
        set(handles.ymaxEdit, 'String', '2');
        set(handles.fitnessEdit, 'String', '-(100*(y-x.^2).^2+(1-x).^2)');
    case 6
        set(handles.fitnessEdit, 'String', '-(10*2+ (x.^2-10*cos(2*pi*x)) + (y.^2-10*cos(2*pi*y)))');
    case 7
        set(handles.xminEdit, 'String', '-500');
        set(handles.xmaxEdit, 'String', '500');
        set(handles.yminEdit, 'String', '-500');
        set(handles.ymaxEdit, 'String', '500');
        set(handles.fitnessEdit, 'String', '-(-x.*sin(sqrt(abs(x))) -y.*sin(sqrt(abs(y))))');
    case 8
        set(handles.fitnessEdit, 'String', '-(x.^2/4000+y.^2/4000 - cos(x).*cos(y/sqrt(2)) +1)');
    case 9
        set(handles.fitnessEdit, 'String', '-(abs(x).^2 + abs(y).^3)');
    case 10
        set(handles.fitnessEdit, 'String', '-(-20*exp(-0.2*sqrt(1/2*(x.^2+y.^2)))-exp(1/2*(cos(2*pi*x)+cos(2*pi*y)))+20+exp(1))');
    case 11
        set(handles.fitnessEdit, 'String', '-(-(sin(x).*(sin(x.^2/pi)).^(2*10) +sin(y).*(sin(2*y.^2/pi)).^(2*10) ))');
    case 12
        set(handles.fitnessEdit, 'String', '-(1*(y-5.1/4/pi^2*x.^2 +5/pi*x - 6).^2 + 10*(1-1/8/pi)*cos(x)+10)');
    case 13
        set(handles.fitnessEdit, 'String', '-(-cos(x).*cos(y).*exp(-( (x-pi).^2 + (y-pi).^2  )))');
    case 14
        set(handles.fitnessEdit, 'String', '-(-cos(x+5).*cos(y+4).*exp(-( (x+5-pi).^2 + (y+4-pi).^2  )))');
    case 15
        set(handles.fitnessEdit, 'String', '-((4-2.1*x.^2+x.^(4/3)).*x.^2+x.*y+(-4+4*y.^2).*y.^2)');
    case 16
        set(handles.xminEdit, 'String', '-2');
        set(handles.xmaxEdit, 'String', '2');
        set(handles.yminEdit, 'String', '-2');
        set(handles.ymaxEdit, 'String', '2');
        set(handles.fitnessEdit, 'String', ' -(100*(y-x.^2).^2+(1-x).^2) +  -2000*(-(sin(x/30).*(sin((x/30).^2/pi)).^(2*10) +sin(y).*(sin(2*(y).^2/pi)).^(2*10) )) ');
    case 17
        set(handles.xminEdit, 'String', '-4');
        set(handles.xmaxEdit, 'String', '4');
        set(handles.yminEdit, 'String', '-4');
        set(handles.ymaxEdit, 'String', '4');
        set(handles.fitnessEdit, 'String', '-(-(sin(x).*(sin(x.^2/pi)).^(2*10) +sin(y).*(sin(2*y.^2/pi)).^(2*10) ))  +  -(-20*exp(-0.2*sqrt(1/2*(x.^2+y.^2)))-exp(1/2*(cos(2*pi*x)+cos(2*pi*y)))+20+exp(1))');
end





% --- Executes during object creation, after setting all properties.
function functionsPopupmenu_CreateFcn(hObject, eventdata, handles)
% hObject    handle to functionsPopupmenu (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: popupmenu controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end



function levelsEdit_Callback(hObject, eventdata, handles)
% hObject    handle to levelsEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of levelsEdit as text
%        str2double(get(hObject,'String')) returns contents of levelsEdit as a double


% --- Executes during object creation, after setting all properties.
function levelsEdit_CreateFcn(hObject, eventdata, handles)
% hObject    handle to levelsEdit (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc
    set(hObject,'BackgroundColor','white');
else
    set(hObject,'BackgroundColor',get(0,'defaultUicontrolBackgroundColor'));
end


