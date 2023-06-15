const readline = require('readline');

class Participant {
  constructor(participantId, name) {
    this.participantId = participantId;
    this.name = name;
    this.score = 0;
  }

  addScore(score) {
    this.score += score;
  }
}

class Question {
  constructor(questionId, qnInWords, options, answer) {
    this.questionId = questionId;
    this.qnInWords = qnInWords;
    this.options = options;
    this.answer = answer;
  }
}

const questionsData = [
  {
    questionId: 1,
    qnInWords: 'Pyetja 1',
    options: ['Përgjigja 1', 'Përgjigja 2', 'Përgjigja 3', 'Përgjigja 4'],
    answer: 2
  },
  {
    questionId: 2,
    qnInWords: 'Pyetja 2',
    options: ['Përgjigja 1', 'Përgjigja 2', 'Përgjigja 3', 'Përgjigja 4'],
    answer: 3
  }
];

function getUserAnswer() {// funksion asinkron qe kthen promise
  const rl = readline.createInterface({
    input: process.stdin, //merr hyrjen standarde
    output: process.stdout//daljen standarde
  });

  return new Promise((resolve) => { //zgejdh promise nga pergjigjja e perdoruesit
    rl.question('Përgjigja juaj: ', (answer) => { //pergjigjja pranohet si argument i funksionit
      rl.close();
      resolve(parseInt(answer)); //kthen pergjigjjen si numer te plote
    });
  });
}

async function startQuiz() { //fillon ekzekutimi ne menyren lineare  deri te getUserAnswer, pra startquiz nuk ekzekutohet menjehre pret per pergjigjje nga useri
  console.log('Quiz fillon! Përgjigju me numrin e opsionit të saktë.');

  const participant = new Participant(1, 'Emri i pjesëmarrësit');
  const questions = questionsData.map(qData => new Question(qData.questionId, qData.qnInWords, qData.options, qData.answer)); //per secilien pyetje krijohet objekti question 

  let totalTime = 0;
  let isTimeUp = false;

  const startTime = Date.now();
  const quizTimeLimit = 60 * 1000; // 1 minutë në milisekonda

  const timeInterval = setInterval(() => {
    const currentTime = Date.now();
    totalTime = Math.floor((currentTime - startTime) / 1000);

    if (currentTime - startTime > quizTimeLimit) {
      console.log('Kenit vonuar mbi 1 minutë në quiz.');
      clearInterval(timeInterval);
      isTimeUp = true;
      endQuiz(participant, totalTime);
    }
  }, 1000);

  for (const question of questions) {
    if (isTimeUp) {
      break;
    }

    console.log(question.qnInWords);
    for (let i = 0; i < question.options.length; i++) {
      console.log(`${i + 1}. ${question.options[i]}`);
    }

    const userAnswer = await getUserAnswer(); //pret deri sa perdoruesi te jep pergjijje dhe kthen vleren e pergjigjjes se perdoruesit

    if (userAnswer === question.answer) { //nese perdoruesi jep pergjigjjen e te dhenave te sakta
      console.log('Përgjigja juaj është e saktë!');
      participant.addScore(1);
    } else {
      console.log
      ('Përgjigja juaj është e gabuar!');
        }
          console.log();
        }
      
        if (!isTimeUp) {
        endQuiz(participant, totalTime);
        }
      }
      
      function endQuiz(participant, totalTime) {
        console.log('Quiz-i ka përfunduar!');
        console.log(`Pikët: ${participant.score}`);
        console.log(`Koha e marrë: ${totalTime} sekonda`);
        console.log('Faleminderit për pjesëmarrjen!');
      }
      
      startQuiz();
