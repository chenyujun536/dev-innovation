from io import TextIOWrapper
import os
import zipfile

sml_path = "\\\\srv004bgc.dir.slb.com\\MaxwellAnalytics\\ArchiveBackup"
local_path = "D:\\Temp\\sml\\2021_perf"
local_result_file = "C:\\Users\\ychen32\\OneDrive - Schlumberger\\Desktop\\sml\\2021_perf_raw.txt"
local_filtered_file = "C:\\Users\\ychen32\\OneDrive - Schlumberger\\Desktop\\sml\\2021_perf_result_filtered.csv"
local_formatted_file = "C:\\Users\\ychen32\\OneDrive - Schlumberger\\Desktop\\sml\\2021_perf_result_formatted.csv"
local_summary_file = "C:\\Users\\ychen32\\OneDrive - Schlumberger\\Desktop\\sml\\2021_perf_result_summary.csv"
local_final_result = "C:\\Users\\ychen32\\OneDrive - Schlumberger\\Desktop\\sml\\2021_perf_result_final.csv"

def getListFolders():
    return os.listdir(sml_path)

def copySMLFilesToLocal():
    count = 0
    listFolders = getListFolders()
    for folder in listFolders:
        path = os.path.join(sml_path, folder)
        if os.path.isdir(path):
            files = os.listdir(path)
            for file in files:
                if "MaxwellSystemMonitor" in file and "@2021" in file and ".zip" in file and not ".txt" in file:
                    try:
                        with zipfile.ZipFile(os.path.join(path, file), 'r') as zip_ref:
                            zip_ref.extractall(local_path)
                        count +=1
                    except:
                        print("process error", file)
                    #break
    print(count, " files extracted")
            #print(path)

def removeUselessFiles():
    files = os.listdir(local_path)
    count = 0
    for file in files:
        if not "ProcessPerformance" in file or "Independent" in file:
            try:
                os.remove(os.path.join(local_path, file))
                count += 1
            except:
                print("process error", file)
    print(count, " files deleted")

class ProcessStatistic:
    def __init__(self, processName):
        self.processname = processName
        self.memoryUsage = {
            "20G":0,
            "25G":0,
            "30G":0,
            "35G":0,
            "40G":0
        }

    def updateOneVMUsage(self, processVM:int):
        if processVM < 2000000000:
            self.memoryUsage["20G"] = self.memoryUsage["20G"] +1
        elif processVM< 2500000000:
            self.memoryUsage["25G"] = self.memoryUsage["25G"] +1
        elif processVM< 3000000000:
            self.memoryUsage["30G"] = self.memoryUsage["30G"] +1
        elif processVM< 3500000000:
            self.memoryUsage["35G"] = self.memoryUsage["35G"] +1
        else:
            self.memoryUsage["40G"] = self.memoryUsage["40G"] +1
    
    def updateVMSummary(self, memKey:str, memValue:int):
        self.memoryUsage[memKey] = self.memoryUsage[memKey] + memValue
    
    def printMe(self, file:TextIOWrapper):
        file.write(self.processname)
        file.write(str(self.memoryUsage))
        file.write('\n')

class FileStatistic:

    def __init__(self, jobName):
        self.jobName = jobName
        self.proc:dict[str, ProcessStatistic] = {}
    
    def updateVM(self, processName:str, processVM:int):
        if not processName in self.proc:
            self.proc[processName] = ProcessStatistic(processName)
        
        self.proc[processName].updateOneVMUsage(processVM)
    
    def printMe(self, file:TextIOWrapper):
        file.write(self.jobName)
        file.write('\n')
        for process in self.proc:
            self.proc[process].printMe(file)


def composeProcessName(processName:str, processTitle:str):
    if len(processTitle)==0:
        return processName
    else:
        return processTitle[:5]



def analyzeOneFile(fileStatistics:FileStatistic, one_file_path:str):
    try:
        with open(one_file_path, 'r') as f:
            lines = f.readlines()
            for line in lines:
                if line.startswith("Time"):
                    continue
                words = line.split(",")
                try:
                    if words[2].isdigit() or words[3].isdigit():
                        continue
                    processName = composeProcessName(words[2], words[3])
                    processVM = int(words[8])
                    fileStatistics.updateVM(processName, processVM)
                except Exception as e:
                    print(e)
    except Exception as e:
        print("error while analyze ", one_file_path)

def analyze():
    files = os.listdir(local_path)
    stat:dict[str,FileStatistic]={}
    count = 0
    for file in files:
        keys = file.split("__ProcessPerformance__")
        fileName = keys[0]
        if not fileName in stat:
            stat[fileName] = FileStatistic(fileName)
        analyzeOneFile(stat[fileName], os.path.join(local_path, file))
        count += 1
        if count%100 == 0:
            print("processed ", count, " files")
    with open(os.path.join(local_path, 'result.txt'), 'w') as f:            
        for file in stat:
            stat[file].printMe(f)

def printProcessName():
    process = set()
    with open(local_result_file, 'r') as fr:
        lines = fr.readlines()
        for line in lines:
            words = line.split(",")
            if "__" in words[0]:
                pass
            else:
                process.add(words[0])
    print(process)

def removeInvalidProcesses():
    #invalidProcesses = ["chrome", "2%", "27%", "36%", "52%", "78%", "80%", "93%", "96%"]
    invalidProcesses = [ 'MSN',
        'Dashb', '[LIZ_', 'Enter', 'PCWAFE~1', 
    '58% c',  'Test ', 'NOC S', '20% c', '95% c', 'File ', '64% c', 
     'MDT.o', '72% c', '30% c', 'www.b', 
    '15% c', 'Slb.FieldSync.EtwConnectorService', '26% c', 'Holla', 
    'qatar', 'em1.p', '77% c', '75% c', 
    '45% c', 'Outlo', 'Power', 'pecel', 'My We', '13% c', '37% c',
     'Disab', '31% c', 'go.mi', '74% c', 'Indic', 'Chann', 'CVX 9',
      '54% c', 'Rig T', 'Cable', 'EDTC-',  '60% c',
       'Slb.FieldSync.ServiceMonitor', '83% c', '90% c', 'Load ', '7122/', 
       '43% c', 'Calib', 'Setup', 'Well ', 'MPSU ', '87% c', 'Sign ', 'Work ', 
       'covid', 'SEC M', 'Slim/', 'Caspe', 'Unit ',  'Clear', 'WellD', 
       'CSlbE', '93% c', 'PNX S', '9% co', 'iaFil', 'Googl', 
       'Error', '14 Su', 
       '16% c', '96% c', 'Heade', 'How w', 'Cost ', '59% c', 'Barre',
        'bet a', 'Beach', 'MAMS-', 'Compl', 'IT 77', '84% c', 
        'One-C',   '19% c', '33% c', 
        '0% co', 'REP12', '48% c', 'Qatar', '17% c',  "What鈡'20G'", 
        '8min ', '65% c', 'PNX', 'MAPC-', '49% c', 
        '63% c', '39% c', '40% c', 'Norma', '1% co', '[NOC_', 'TeleS',  
        'Manda', 'chrome', '9GAG', 'Tool ', 'charl', 'Good ', 
        'slb p', '18% c', 'State', 'ADV r', '2% co', '44% c', 'IBC-C', 'Enabl',
         'Run M', 'Repla', 'Crash', 'Baron',  
         'Charl', '42% c', 'Surfa', 
          'ThmDataSender', 'DnI I', '12% c', '73% c', 'MASTE', '[1] -', 
          'EQF-4', 'Creat', 
          '25% c', '71% c',           
          'Home ', 'LIZ 5', 'ODF C', 'Movem', 'Input', 'PcEWafeServer',
           'How d', 'Pass ', 'Speed', 'Schlu', 'Movie', 'we - ',
            'Relay', 'Wait.', '57% c', '鈥楿n', 'selec', 'sitec', 
             '70% c', '24% c', 'marin', "USI 鈡'20G'", 'Surve', '47% c', '81% c', 
             'LiveO', 'GPIC-', '27% c', 'XPS-C', 'd2a', 'WOExp', '55% c', 'auto ',
              'wlrts', 'Untit', '99% c', 'Slb.FieldSync.FileCollectorService', 
              'VOLTA', 'SignO', 'SlbCu', 'LIZ_5', 'WSSec', 'PNX N', 'OneDr', '23 te',
               'DEC P', '21% c', '5% co', 'Multi', 'ToolS', 
               'New M', 'PERFI', '29% c', 'Add C', 'PGGC-', 'NOR O', '8% co', 
               'Netwo', '46% c', 
               '2 Run', 'Data',  'Milit', 'PNX P', '50% c', 
               '16/4-', 'Whats', '11% c', 'live ', '69% c', 'Equip', 'C',
               'Cisco', '66% c', 'Reddi', 'Worki', '23% c', 
                'XPT -', '10% c', 'remot', '91% c', 
               'Warni', 'Share', 
                'Mail ', '41% c', 'Offic', 'MNT12', 'Check', 'heave', '85% c', 
                'Acqu4', '14% c', 'PcEWafeServices', 'm to ', 'adobe', 
                '61% c', 'Opera',  'Save ', 'Kuwai', 'Messa',
                 'eWAFE', 'cbl_j', '89% c', 'NEXT ', 'Windo',
                  'Stati', 'So it', 'PEX-H', 
                  'PDF t', 'InTou', 'Shari', '51% c', 
                  '62% c', 'Print', '97% c', '4% co', 
                  'SMFT1', 'Setti', 'WARNI', 'Selec', 'Folde', '98% c', 'Tesla',
                   'REP11', 'Digit', 'Local', 'LCRT ', 
                   'Inval', 'AH-10', 'Corre', 'Kogni', '88% c', 'Optim', '52% c',
                    'EFPS ', '36% c', '80% c', 'ECH-K', '28% c', 'ECH-M', 'hspmd',
                     'Forma', 'WLJAR', 'PDF2TIFF',  '6.125', 
                      'NewUI', 'High ', '32% c', 'erl', 
                      '94% c', 'Valid', 'MRSR', '38% c', 'HRGD-', 'Prope', 'MDT T', 
                      'Planc', 'Gener', '79% c', '92% c', 'Disco', '7% co', 'UIHub', 
                      'europ', '68% c', '78% c', 'Larmo', '[IMPu', 'intou', 
                      'Steve', '35% c', '[01] ', '[PD-A', '[NDT_', 
                      'Remot', 'Slow ', '82% c', 
                       'Drag', '100%', '6% co', 'Env', 'Autom', 'Write', 'BIN_D', 'Name ',
                        'Santo', 'Item ', 'hi - ', '67% c', 'ADBE ', '1 Int',
                         'Lewis', 'auth.', 'Open ', 'Verif', 
                         '(8)','(9)','(10)','(11)', '9.625', 'myHub', 'Myfi', 'https', 'New T', 'Welco', 'Globa',
                          'Firmw', 'Flow ', 'ec001', '76% c', 'ZM fu', '53% c', 
                          'Curve',  'He ca', '56% c', 'Possi', '3% co', 
                          'Parts', 'Crimi', '1 ite', '18 it', 'PNX F', '34% c', 'SlimX',
                            'Vinta', 'Steat', '86% c', '22% c']
    process = set()

    with open(local_result_file, 'r') as fr:
        with open(local_filtered_file, 'w') as fw:
            lines = fr.readlines()
            for line in lines:
                invalidProcess = list(filter(lambda x: line.startswith(x), invalidProcesses))
                if len(invalidProcess) == 0:
                    fw.write(line)

def formatFile(sourcefile:str, targetfile:str):
    # from this line
    # Acqui{'20G': 10, '25G': 0, '30G': 0, '35G': 0, '40G': 0}
    # to this line
    # Acqui,20G,10,25G,0,30G,0,35G,0,40G,0
    with open(sourcefile, 'r') as fr:
        with open(targetfile, 'w') as fw:
            lines = fr.readlines()
            for line in lines:
                line = line.replace("{'",",").replace(":",",").replace("'","").replace(" ","").replace("}","")
                fw.write(line)
                
        

def countVMDistributionByProcessName():
    stat:dict[str,ProcessStatistic]={}
    with open(local_formatted_file, 'r') as fr:
        lines = fr.readlines()
        for line in lines:
            if "__" in line:
                pass
            else:                
                words = line.split(",")
                if not words[0] in stat:
                    stat[words[0]] = ProcessStatistic(words[0])
                stat[words[0]].updateVMSummary(words[1],int(words[2]))
                stat[words[0]].updateVMSummary(words[3],int(words[4]))
                stat[words[0]].updateVMSummary(words[5],int(words[6]))
                stat[words[0]].updateVMSummary(words[7],int(words[8]))
                stat[words[0]].updateVMSummary(words[9],int(words[10]))
        with open(local_summary_file, 'w') as fw:
            for proc in stat:
                stat[proc].printMe(fw)
        
                    

def postProcessing():
    #printProcessName()
    #removeInvalidProcesses()
    #formatFile(local_filtered_file, local_formatted_file)
    #countVMDistributionByProcessName()
    formatFile(local_summary_file, local_final_result)

def main():
    #copySMLFilesToLocal()
    #removeUselessFiles()
    #analyze()
    postProcessing()


if __name__ == '__main__':
    main()

