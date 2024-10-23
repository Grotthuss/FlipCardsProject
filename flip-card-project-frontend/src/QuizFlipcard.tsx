import React from 'react';
import './QuizFlipcard.css';
import { useParams } from 'react-router-dom';
import { Errors } from "./errorEnums";

interface FlipCardData {
    Question: string;
    Concept: string;
    Mnemonic: string;
}

const QuizFlipCard: React.FC = () => {
    const { title } = useParams<{ title: string }>();
    const [cards, setCards] = React.useState<FlipCardData[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);
    const [inputValue, setInputValue] = React.useState('');
    const [currentCardIndex, setCurrentCardIndex] = React.useState<number>(0);
    const [score, setScore] = React.useState<number>(0);
    const [feedback, setFeedback] = React.useState<string | null>(null);
    const [flipped, setFlipped] = React.useState<boolean>(false);

    const flipAnsweredCard = (event: React.ChangeEvent<HTMLInputElement>) => {
        setInputValue(event.target.value);
    };

    const handleAnswerSubmit = () => {
        const correctAnswer = cards[currentCardIndex].Concept.toLowerCase();

        if (inputValue.toLowerCase() === correctAnswer) {
            setFeedback("Correct!");
            setScore(prevScore => prevScore + 1);
        } else {
            setFeedback(`Incorrect! The correct answer is: ${cards[currentCardIndex].Concept}`);
        }

        setInputValue('');
        setFlipped(true);
    };

    const handleNextCard = () => {
        setFlipped(false);

        if (currentCardIndex < cards.length - 1) {
            setCurrentCardIndex(prevIndex => prevIndex + 1);
            setFeedback(null);
        } else {
            setFeedback(`Quiz finished! Your score: ${score + (inputValue.toLowerCase() === cards[currentCardIndex].Concept.toLowerCase() ? 1 : 0)} / ${cards.length}`);
        }
    };

    React.useEffect(() => {
        const fetchCards = async () => {
            if (!title) {
                setError(Errors.TITLE);
                setLoading(false);
                return;
            }
            try {
                const response = await fetch(`https://localhost:44372/api/Home/${title}/GetCardSet`);
                if (!response.ok) {
                    throw new Error(Errors.NETWORK);
                }
                const data = await response.json();
                if (data && Array.isArray(data._flipcards_list)) {
                    setCards(data._flipcards_list);
                } else {
                    setError(Errors.FORMAT + JSON.stringify(data));
                }
            } catch (error) {
                setError(Errors.CARDS + (error as Error).message);
            } finally {
                setLoading(false);
            }
        };

        fetchCards();
    }, [title]);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="flip-card-page">
            <h2>Quiz Score: {score} / {cards.length}</h2>
            <div className="cards-container">
                {cards.length > 0 && currentCardIndex < cards.length ? (
                    <>
                        <div className={`flip-card ${flipped ? 'flipped' : ''}`} id={`flipCard-${currentCardIndex}`}>
                            <div className="flip-card-inner">
                                <div className="flip-card-front">
                                    <p className="card-question">{cards[currentCardIndex].Question}</p>
                                    <h2>{cards[currentCardIndex].Mnemonic}</h2>
                                </div>
                                <div className="flip-card-back">
                                    {flipped && <h2>{cards[currentCardIndex].Concept}</h2>}
                                </div>
                            </div>
                        </div>
                        <div className="input-container">
                            <input
                                type="text"
                                value={inputValue}
                                onChange={flipAnsweredCard}
                                placeholder="Your answer here"
                                disabled={flipped}
                            />
                            <button onClick={handleAnswerSubmit} disabled={flipped}>Submit Answer</button>
                        </div>
                        <div className="feedback-message-container">
                            {feedback && <div className="feedback-message">{feedback}</div>}
                        </div>
                        {flipped && (
                            <button onClick={handleNextCard}>Next</button>
                        )}
                    </>
                ) : (
                    <div className="feedback-message">Quiz finished! Your score: {score} / {cards.length}</div>
                )}
            </div>
        </div>
    );
};

export default QuizFlipCard;
