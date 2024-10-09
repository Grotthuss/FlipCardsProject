import React from 'react';
import './FlipCard.css';
import { useParams } from 'react-router-dom';

interface FlipCardData {
    Id: number;
    Question: string;
    Concept: string;
    Mnemonic: string;
}

const FlipCard: React.FC = () => {
    const { title } = useParams<{ title: string }>();
    const [cards, setCards] = React.useState<FlipCardData[]>([]);
    const [loading, setLoading] = React.useState<boolean>(true);
    const [error, setError] = React.useState<string | null>(null);

    const handleFlip = (id: number) => {
        const flipCard = document.getElementById(`flipCard-${id}`);
        if (flipCard) {
            flipCard.classList.toggle('flipped');
        }
    };

    React.useEffect(() => {
        const fetchCards = async () => {
            if (!title) {
                setError('Set title is undefined. Please check your URL.');
                setLoading(false);
                return;
            }
            try {
                const response = await fetch(`https://localhost:44372/api/Home/${title}`);

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                const data = await response.json();

                if (data && Array.isArray(data._flipcards_list)) {
                    setCards(data._flipcards_list);
                } else {
                    setError("Unexpected response format: " + JSON.stringify(data));
                }
            } catch (error) {
                setError('Error fetching cards: ' + (error as Error).message);
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

    if (!Array.isArray(cards) || cards.length === 0) {
        return <div>No cards available</div>;
    }

    return (
        <div className="container">
            {cards.map((card) => (
                <div
                    className="flip-card"
                    key={card.Id}
                    id={`flipCard-${card.Id}`}
                    onClick={() => handleFlip(card.Id)}
                >
                    <div className="flip-card-inner">
                        <div className="flip-card-front">
                            <p className="card-question">{card.Question}</p>
                            <h2>{card.Mnemonic}</h2>
                        </div>
                        <div className="flip-card-back">
                            <h2>{card.Concept}</h2>
                        </div>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default FlipCard;
