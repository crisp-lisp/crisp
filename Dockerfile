FROM mono:5.20.1.34 
# 4.8.0.495
RUN mkdir /opt/crisp
COPY . /opt/crisp
WORKDIR /opt/crisp
RUN nuget update -self
RUN nuget restore
RUN xbuild /p:Configuration=Release Crisp.sln
WORKDIR /opt/crisp/Packet/bin/Release
CMD ["mono", "./Packet.exe"]
