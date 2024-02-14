FROM mono:4.8.0.495
RUN mkdir /opt/crisp
COPY . /opt/crisp
WORKDIR /opt/crisp
RUN nuget restore
RUN xbuild Crisp.sln
CMD ["ls", "/opt/crisp/Packet"]
