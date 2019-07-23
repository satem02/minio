# dotnet-template-repository

Projelerde her defasında oluşturduğumuz dosya yapısı veya diğer projelerden kopyaladığımız core katmanı vs. gibi standart işlemleri için bir tane base repository oluşturup sonraki repositoryleri bu base repository den türetme imkanını github sağlamaktadır. Bu projede buna örnek amaçlı docker üzerinde dotnet core web api projeleri için bir base repositorydir.   

1 - git clone https://github.com/satem02/net-core-api-template-repository.git  
2 - cd net-core-api-template-repository  
3 - docker-compose -f "docker-compose.yml" up -d --build  
4 - http://localhost:5000/api/values  
