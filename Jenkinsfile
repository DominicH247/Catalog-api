// using custom docker image for jenkins with .net core 5.0 installed - docker pull dominich212/jenkins-dotnet-5.0:v1
pipeline {
    agent any

    stages{
        stage("build") {
            steps {
                sh '''
                echo "Restoring project..."
                dotnet restore
                echo "Building project..."
                dotnet build
                '''
            }
        }

        stage("test") {
            steps {
                sh '''
                echo "Running unit tests..."
                dotnet test
                '''
            }
        }

        stage("deploy") {
            steps {
                echo 'deploying application...'
            }
        }
    }
}
